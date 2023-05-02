using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMechanic.Extensions
{
    /// <summary>
    /// Credit:
    /// https://mikhail.io/2016/01/monads-explained-in-csharp/
    /// https://mikhail.io/2018/07/monads-explained-in-csharp-again/
    /// https://www.pluralsight.com/tech-blog/maybe
    ///
    /// Usage:
    /// var some_value = my_repo.Get<T>(id).ToMaybe();
    /// some_value.Case(some:(x)=> { /*... do something*/ }, none: _ => { /*... do something*/ } // Null is intrinsically handled for you.
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Maybe<T>
    {
        private readonly IEnumerable<T> values;

        public bool HasValue => !values.IsNullOrEmpty();

        public static implicit operator Maybe<T>(T value) => Some(value);

        // Only usable in C# 7.3 or higher... :'(
        //public static implicit operator Maybe<T?>(T? value) => value.ToMaybe();

        public static Maybe<T> Some(T value) => value == null
             ? throw new ArgumentNullException($"Cannot add a null value for a reference type like {typeof(T).Name}")
             : new Maybe<T>(new[] { value });

        public static Maybe<T> None => new Maybe<T>(new T[0]);

        public T Value => HasValue
                ? values.Single()
                : throw new Exception($"Maybe of type {typeof(T).Name} does not have a value");

        /// <summary>
        /// Constructor which handles the case where the value is a single value.
        /// Internally, I'm treating it as a collection on purpose.
        /// </summary>
        /// <param name="value">The value.</param>
        public Maybe(T value) => values = Enumerable.Repeat(value, 1);

        //private Maybe(IEnumerable<T> values) => this.values = values;
        private Maybe(params T[] values) => this.values = values;

        public T ValueOrDefault(T fallback_value) => !HasValue
                ? fallback_value
                : values.Single();

        public T ValueOrThrow(Exception ex) => HasValue
                ? Value
                : throw ex;

        /// <summary>
        /// Handle the cases where there is some value or there is none:
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="some"></param>
        /// <param name="none"></param>
        /// <returns></returns>
        public U Case<U>(Func<T, U> some, Func<U> none) => HasValue
                ? some(Value)
                : none();

        /// <summary>
        /// Handle the cases where there is some value or there is none:
        /// </summary>
        /// <param name="if_some_value">Some Value</param>
        /// <param name="if_no_value">None</param>
        /// <returns></returns>
        public Maybe<T> Case(Action<T> if_some_value, Action if_no_value)
        {
            if (HasValue)
                if_some_value(Value);
            else
                if_no_value();
            return this;
        }

        /// <summary>
        /// Performs a side-effect if there is a value
        ///
        /// Sample Usage:
        ///
        /// var maybeAccount = repository.Find(accountId);
        /// maybeAccount.IfSome(account =>
        /// {
        ///     account.LastUpdated = DateTimeOffset.UtcNow;
        ///     repository.Save(account);
        /// });
        ///
        /// </summary>
        public Maybe<T> IfSome(Action<T> if_some_value)
        {
            if (HasValue)
            {
                if_some_value(Value);
            }

            return this;
        }

        /// <summary>
        /// Transforms the return value to another type if some value is found
        /// </summary>
        public U IfSome<U>(Func<T, U> if_some_value)
        {
            return HasValue && if_some_value != null
               ? if_some_value(Value)
               : default;
        }
        /// <summary>
        /// Alters the return value type if some value is found
        ///
        /// Usage:
        ///
        /// var orders = await repo.GetOrders(123);
        ///
        /// var result = orders.First().ToMaybe();
        ///
        /// result.IfSome((o)=> {
        ///     return o.MapToDto<PoDetail>(new PODetail{});
        /// }
        /// </summary>
        public T IfSome(Func<T, T> if_some_value)
        {
            return HasValue && if_some_value != null
                ? if_some_value(Value)
                : Value;
        }
        /// <summary>
        /// Usage:
        /// public static string GetUserCulture2(int userId)
        /// => _userRepository.FindById(userId)
        /// .Bind(user => user.Preferences) // FlatMap
        /// .Map(prefs => prefs.Culture)
        /// .IfNone("en-US"); // default culture
        ///
        /// </summary>
        /// <param name="if_no_value_fn"></param>
        /// <param name="fallback_value"></param>
        /// <returns></returns>
        public T IfNone(Func<T> if_no_value_fn)
        {
            return !HasValue && if_no_value_fn != null
                ? if_no_value_fn()
                : Value;
        }
        /// <summary>
        /// If Null, fallback to default of T
        ///
        /// The point of <see cref="Maybe{T}"/> is to hide and defer Null Reference Exceptions so that we can handle them at the appropriate time.
        ///
        /// This is one of those times where we can tell the caller what to do with the value without throwing up and logging, necessarily.
        ///
        /// </summary>
        public T IfNone(T fallback_value = default(T)) =>
            !HasValue
                ? fallback_value
                : Value;

        /// <summary>
        /// Sample Usage:
        /// Maybe<Shipper> shipperOfLastOrderOnCurrentAddress =
        ///     repo.GetCustomer(customerId)
        ///     .Bind(c => c.Address)
        ///     .Bind(a => repo.GetAddress(a.Id))
        ///     .Bind(a => a.LastOrder)
        ///     .Bind(lo => repo.GetOrder(lo.Id))
        ///     .Bind(o => o.Shipper);
        /// </summary>
        public Maybe<U> Bind<U>(Func<T, Maybe<U>> func)// where U : class
        {
            var value = values.SingleOrDefault();
            return value != null ? func(value) : new Maybe<U>();
        }

        /// <summary>
        /// Map a maybe to another type:
        /// Maybe<string> maybeFirstName = maybeAccount.Map(account => account.FirstName);
        /// Maybe<IList<string>> emails = maybeAccount.Map(account => repository.GetEmailAddresses(account));
        ///
        /// emails.Case(
        ///     some: ()=> {... do work... },
        ///     none: ()=> {... handle any null or empty cases here }
        /// </summary>
        public Maybe<U> Map<U>(Func<T, Maybe<U>> map_to) => HasValue
                ? map_to(Value)
                : Maybe<U>.None;

        /// <summary>
        /// Map a maybe to another type:
        /// Maybe<string> maybeFirstName = maybeAccount.Map(account => account.FirstName);
        /// Maybe<IList<string>> emails = maybeAccount.Map(account => repository.GetEmailAddresses(account));
        ///
        /// emails.Case(
        ///     some: ()=> {... do work... },
        ///     none: ()=> {... handle any null or empty cases here }
        /// </summary>
        public Maybe<U> Map<U>(Func<T, U> map_to) => HasValue
                ? Maybe.Some(map_to(Value))
                : Maybe<U>.None;
    }

    public static class Maybe
    {
        public static Maybe<T> Some<T>(T value) => Maybe<T>.Some(value);
    }

    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T instance_of_class)
        //where T : class
        {
            return instance_of_class != null
                ? Maybe.Some(instance_of_class)
                : Maybe<T>.None;
        }

        public static Maybe<string[]> ToMaybe(this string[] text)
        {
            return !text.IsNullOrEmpty()
                ? Maybe.Some(text)
                : Maybe<string[]>.None;
        }

        public static Maybe<IEnumerable<T>> ToMaybe<T>(this IEnumerable<T> array_of_instances)
        //where T : class
        {
            return !array_of_instances.IsNullOrEmpty()
                ? Maybe.Some(array_of_instances)
                : Maybe<IEnumerable<T>>.None;
        }

        public static Maybe<T> ToMaybe<T>(this T? nullable_object)
            where T : struct
        {
            return nullable_object.HasValue
                ? Maybe.Some(nullable_object.Value)
                : Maybe<T>.None;
        }

        public static Maybe<string> NoneIfEmpty(this string text)
        {
            return string.IsNullOrEmpty(text)
                ? Maybe<string>.None
                : Maybe.Some(text);
        }

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> collection)
        //where T : class
        {
            return collection.FirstOrDefault().ToMaybe();
        }

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T?> collection)
            where T : struct
        {
            return collection.FirstOrDefault().ToMaybe();
        }

        public static IEnumerator<T> GetEnumerator<T>(this Maybe<T> maybe) => ((IEnumerable<T>)maybe.Value).GetEnumerator();
    }

    /* Uncomment when tuples are the new legacy:
     * Credit: https://functionalprogramming.medium.com/revisiting-maybe-monad-using-c-8-0-pattern-matching-d21ee805b114
     */

    //public abstract class Maybe<T>
    //{
    //    public abstract Maybe<T1> Map<T1>(Func<T, T1> f);
    //    public abstract TResult MatchWith<TResult>((Func<TResult> none, Func<T, TResult> some) pattern);
    //    public Maybe<T1> Bind<T1>(Func<T, Maybe<T1>> f) =>
    //       MatchWith((
    //               none: () => new None<T1>(),
    //               some: (v) => f(v)
    //           ));
    //}
    //public class None<T> : Maybe<T>
    //{
    //    public None() { }
    //    public override TResult MatchWith<TResult>((Func<TResult> none, Func<T, TResult> some) pattern) => pattern.none();
    //    public override Maybe<T1> Map<T1>(Func<T, T1> f) => new None<T1>();
    //}

    //public class Some<T> : Maybe<T>
    //{
    //    private readonly T value;
    //    public Some(T value) => this.value = value;
    //    public override TResult MatchWith<TResult>((Func<TResult> none, Func<T, TResult> some) pattern) => pattern.some(value);
    //    public override Maybe<T1> Map<T1>(Func<T, T1> f) => new Some<T1>(f(value));

    //}
}