using System;
using System.Collections.Generic;


namespace CodeMechanic.Extensions
{
    public class DateConstants
    {
        public const string FourHandsNull = "12/30/1899";
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    }

    public static class TypeExtensions
    {
        /// <summary>
        /// Destroy any Disposable Object
        /// 
        /// I'm throwing this in here to round out my With<T>(Action<T> patch) and Map<T,U>(Func<T,U> mappingfn) implementations.
        /// This lets us destroy older versions of objects without having to worry about deep-copying (think Immer)
        /// 
        /// Usage:
        /// 
        ///     older_order.With(previous=> {
        ///         var updated_order = new Order(){
        ///             ExpectedShipDate =  previous.ExpectedShipDate = DateTime.Now.With(dt=>dt.Days(7)); 
        ///             // ... some other updates
        ///         }
        ///     })
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="disposable_object"></param>
        /// <returns></returns>
        public static T Destroy<T>(this T disposable_object) where T : IDisposable
        {
            // How hard is this?  Not very.
            disposable_object?.Dispose();
            // Safely return an empty object
            return default;
        }

        /*
         * 
         *          CONVERTERS
         *          
         */
        public static int ToInt(this string text, int fallback = default)
        {
            return int.TryParse(text, out int result) ? result : fallback;
        }

        public static short ToShort(this string text, short fallback = default)
        {
            return short.TryParse(text, out short result) ? result : fallback;
        }

        public static double ToDouble(this string text, double fallback = default)
        {
            return double.TryParse(text, out double result) ? result : fallback;
        }

        public static decimal ToDecimal(this string text, decimal fallback = default)
        {
            return decimal.TryParse(text, out decimal result) ? result : fallback;
        }

        public static long ToLong(this string text, long fallback = default)
        {
            return long.TryParse(text, out long result) ? result : fallback;
        }

        public static float ToFloat(this string text, float fallback = default)
        {
            return float.TryParse(text, out float result) ? result : fallback;
        }

        public static DateTime? ToDateTime(this string dtString, DateTime? fallback = null)
        {
            if (fallback == null || !fallback.HasValue)
                fallback = DateConstants.Epoch;

            return DateTime.TryParse(dtString, out DateTime result) ? result : fallback;
        }

        public static int ToBit(this string text, int fallback = 0)
        {
            return text.ToInt() == 1 ? 1 : fallback;
        }

        public static bool ToBoolean(this string text, bool fallback = default)
        {
            return bool.TryParse(text, out bool result) ? result : fallback;
        }

        public static string ToYesOrNo(this bool value) => value ? "Y" : "N";

        /// <summary>
        /// (VS 2022/ 2015 SHIM) 
        /// This is a replacement for the '?.' null coaslesce or 'Elvis operator'.
        /// This is here because VB can implicitly convert NULL object values like the Request["some_param"], but C# will not.
        /// This also prevents trying to make if-null-else logic in our ASPX files, and instead lets us call one method to make a type conversion, like so: 
        /// 
        /// <%= r["orderdatetime"].TryGet(odt=>odt.ToString().ToDateTime().Value) %>
        /// </summary>
        public static TValue TryGet<TObject, TValue>(this TObject obj, Func<TObject, TValue> getter, TValue fallback = default)
        where TObject : class
        {
            try
            {
                // TODO: this doesn't properly get the correct type on the lefthand side when TOject actually is a string.
                if (typeof(TObject) == typeof(string))
                {
                    string value = obj as string;
                    return string.IsNullOrWhiteSpace(value) ? fallback : getter(obj);
                }

                return obj == null ? fallback : getter(obj);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /**
         * 
         *          MODIFIERS
         * 
         */
        /// <summary>        
        /// Update properties on a long, nested object easily.
        /// 
        /// Usage:
        ///   your.ultralarge.complex.infragistics.winforms.grid.table.row.With(row=> {
        ///      row.Text = "hello there!"
        ///      row.Color = "blue"
        ///      ...
        ///      };
        ///</summary>
        public static T With<T>(this T obj, Action<T> patch)
        {
            patch(obj);
            return obj;
        }

        /// <summary>
        /// Mutates an instance of T, where T can be another instance of T
        /// 
        /// Usage:
        /// var clone = person.Map(p1=> { 
        /// 
        ///     var p2 = p1.MemberwiseClone();
        /// 
        ///     p2.Name = "Bob";
        ///     p2.Age++;
        ///     return p2;
        /// }
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Mutate<T>(this T obj, Func<T, T> patch)
        {
            // TODO: Deep clone, like Immer
            var val = patch(obj);
            return val;
        }

        /// <summary>
        /// Map a Source class to a Target
        /// except now it's between two different classes
        /// </summary>
        public static TResult Map<TSource, TResult>(
            this TSource source,
            Func<TSource, TResult> map
        ) => map(source);

        // TODO: Switch on conditions being true, running the first one to be true on the associated Action.
        public static void Switch<T>(this T value, Func<Func<T, bool>, Action<T>>[] funcs)
        {
            throw new NotImplementedException();
        }


        public static List<T> AsList<T>(this T obj) => obj.Map(o => new List<T> { o });

        public static T[] AsArray<T>(this T obj) => obj.Map(o => new List<T> { o }.ToArray());
    }


}