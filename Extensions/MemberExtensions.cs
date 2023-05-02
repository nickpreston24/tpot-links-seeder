using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace CodeMechanic.Advanced.Extensions
{
    public static class MemberExtensions
    {
        private static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
        private static readonly string invalidExpressionMessage = "Invalid expression.";


        /// <summary>
        /// This is temporary, just to get the values of the left and right hand sides of a given expression.
        /// IF there's another solution, I'm all ears...
        /// </summary>
        public static List<KeyValuePair<string, object>> GetExpressionParts<T>(
            this Expression<Func<T, object>>[] props)
        {
            var lookup = new List<KeyValuePair<string, object>>();

            foreach (var expression in props)
            {
                Expression body = expression.Body;
                if (body is ConstantExpression ce)
                {
                    string key = expression.Parameters.First().Name;
                    var value = ce.Value;
                    lookup.Add(new KeyValuePair<string, object>(key, value));
                }

                else if (body is UnaryExpression ue)
                {
                    Expression something = (ConstantExpression)(((UnaryExpression)body).Operand);

                    string raw_value = something.ToString();
                    Type prop_type = something.Type;

                    string key = expression.Parameters.First().Name;

                    var value = Convert.ChangeType(raw_value, prop_type);

                    lookup.Add(new KeyValuePair<string, object>(key, value));
                }
                else
                {
                    var me = body as MemberExpression;
                    if (null == me) continue;

                    string key = me.Member.Name;

                }
            }

            return lookup;
        }

        public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression) => GetMemberName(expression.Body);

        public static string GetMemberName<T>(this Expression<Func<T, object>> expression) => GetMemberName(default, expression);

        public static IEnumerable<string> GetMemberNames<T>(this T instance, params Expression<Func<T, object>>[] expressions) => expressions.Select(expression => GetMemberName(expression.Body)).ToList();

        public static IEnumerable<string> GetMemberNames<T>(this Expression<Func<T, object>>[] expressions) => GetMemberNames(default, expressions);

        public static string GetMemberName<T>(this T instance, Expression<Action<T>> expression) => GetMemberName(expression.Body);

        public static string GetMemberName<T>(this Expression<Action<T>> expression) => GetMemberName(default, expression);

        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(expressionCannotBeNullMessage);
            }
            if (expression is MemberExpression)
            {
                // Reference type property or field
                MemberExpression memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }
            if (expression is MethodCallExpression)
            {
                // Reference type method
                MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }
            if (expression is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }
            throw new ArgumentException(invalidExpressionMessage);
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                MethodCallExpression methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }
            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
    }



    /// <summary>
    ///     
    /// Credit: Jon skeet
    /// Source: https://stackoverflow.com/questions/6998523/how-to-get-the-value-of-a-constantexpression-which-uses-a-local-variable
    /// </summary>
    public class Visitor : ExpressionVisitor
    {
        protected override Expression VisitMember
            (MemberExpression memberExpression)
        {
            // Recurse down to see if we can simplify...
            var expression = Visit(memberExpression.Expression);

            // If we've ended up with a constant, and it's a property or a field,
            // we can simplify ourselves to a constant
            if (expression is ConstantExpression)
            {
                object container = ((ConstantExpression)expression).Value;
                var member = memberExpression.Member;
                if (member is FieldInfo)
                {
                    object value = ((FieldInfo)member).GetValue(container);
                    return Expression.Constant(value);
                }
                if (member is PropertyInfo)
                {
                    object value = ((PropertyInfo)member).GetValue(container, null);
                    return Expression.Constant(value);
                }
            }
            return base.VisitMember(memberExpression);
        }
    }

}
