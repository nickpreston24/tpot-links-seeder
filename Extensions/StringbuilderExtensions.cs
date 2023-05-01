using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CodeMechanic.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AsStringBuilder(this string text) => new StringBuilder(text);

        public static StringBuilder RemoveFromEnd(this StringBuilder sb, int number_of_characters) =>
            sb.Length >= number_of_characters
                ? sb.With(s => s.Length -= number_of_characters)
                : sb;

        public static StringBuilder Prepend(this StringBuilder sb, string line) => sb.Insert(0, line);

        /// <summary>
        /// Append only when a condition is met
        /// </summary>
        public static StringBuilder AppendIf<T>(
            this StringBuilder builder
            , Func<T, bool> predicate
            , params T[] values)
        {
            var list = values ?? Enumerable.Empty<T>();
            foreach (var value in list.Where(value => predicate(value)))
            {
                builder.Append(value);
            }

            return builder;
        }

        public static StringBuilder AppendEach<T>(
            this StringBuilder builder,
            IEnumerable<T> collection,
            Func<T, string> selector = null,
            string delimiter = " ")
        {
            return collection
                .Aggregate(builder, (_, next_value) =>
                {
                    selector = selector.TryGet(selector_fn => selector_fn);
                    string next_line = selector(next_value);
                    builder.Append($"{next_line}{delimiter}");
                    return builder;
                })
                .RemoveFromEnd(delimiter.Length);
        }
    }
}
