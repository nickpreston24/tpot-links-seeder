using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
// using CodeMechanic.Advanced.Extensions;
using CodeMechanic.Extensions;

namespace CodeMechanic.Advanced.Extensions
{
    public static class RegexExtensions
    {
        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
                   new Dictionary<Type, ICollection<PropertyInfo>>();

        /// <summary>
        /// Takes a dictionary full of Regex patterns (or words) and swaps those values with whatever you set as the .Value.
        /// 
        /// <usage>
        /// So, for example, a dictionary like this:
        /// 
        /// var replacements = new Dictionary<..>{ { "\d+", "hello there!"}, {"Order", "66"}  }
        /// 
        /// ... and a text string like this:
        /// 
        /// string text = "Order was valued at $100.00";
        /// var altered_text = text.ReplaceAll(replacements);
        /// 
        /// Should look something like:
        /// 
        /// `66 was valued at $hello there!.hello there!`
        /// 
        /// This can be used to do quick (but not comprehensive) replacements to format things like:
        /// * Random Unicode chars you don't want
        /// * Extra spaces
        /// * Other garbage like CLRF
        /// 
        /// It does have a flaw in that the more you replace things, the less reliable it can be, especially if your replacements replace OTHER replacements.  So, tread lightly...
        /// </usage>
        /// </summary>
        public static string[] ReplaceAll(
            this string[] lines,
            Dictionary<string, string> replacementMap
        )
        {
            Dictionary<string, string> map = replacementMap.Aggregate(
                new Dictionary<string, string>(),
                (modified, next) =>
                {
                    // Sometimes in JSON \ have to be represented in unicode.  This reverts it.
                    string fixedKey = next.Key.Replace("%5C", @"\").Replace(@"\\", @"\");
                    string fixedValue = Regex.Replace(next.Value, @"\""", "'");

                    modified.Add(fixedKey, fixedValue);
                    return modified;
                }
            );

            List<string> results = new List<string>();

            foreach (string line in lines)
            {
                string modified = line;
                foreach (KeyValuePair<string, string> replacement in map)
                {
                    modified = Regex.Replace(
                        modified,
                        replacement.Key,
                        replacement.Value,
                        RegexOptions.IgnoreCase
                    );
                }
                results.Add(modified);
            }

            return results.ToArray();
        }

        public static string[] ReplaceAll(
            this string[] lines
            , params (string, string)[] replacements
        )
        {
            var dict = replacements.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            return lines.ReplaceAll(dict);
        }

        public static List<T> Extract<T>(
           this string text,
           string regex_pattern,
           bool enforce_exact_match = false,
           bool debug = false,
           RegexOptions options = RegexOptions.None
        )
        {
            var collection = new List<T>();

            // If we get no text, throw if we're in devmode (debug == true)
            // If in prod, we want to probably return an empty set.
            if (string.IsNullOrWhiteSpace(text))
                return debug
                    ? throw new ArgumentNullException(nameof(text))
                    : collection;

            // Get the class properties so we can set values to them.
            var props = _propertyCache.TryGetProperties<T>();

            // If in prod, we want to probably return an empty set.
            if (props.Count == 0)
                return debug
                    ? throw new ArgumentNullException($"No properties found for type {typeof(T).Name}")
                    : collection;

            var errors = new StringBuilder();

            if (options == RegexOptions.None)
                options =
                    RegexOptions.Compiled
                    | RegexOptions.IgnoreCase
                    | RegexOptions.ExplicitCapture
                    | RegexOptions.Singleline
                    | RegexOptions.IgnorePatternWhitespace;

            var regex = new Regex(regex_pattern, options, TimeSpan.FromMilliseconds(250));

            var matches = regex.Matches(text).Cast<Match>();


            matches.Aggregate(
                collection,
                (list, match) =>
            {
                if (!match.Success)
                {
                    errors.AppendLine(
                            $"No matches found! Could not extract a '{typeof(T).Name}' instance from regex pattern:\n{regex_pattern}.\n"
                        );
                    errors.AppendLine(text);

                    var missing = props
                    .Select(property => property.Name)
                        .Except(regex.GetGroupNames(), StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                    if (missing.Length > 0)
                    {
                        errors.AppendLine("Properties without a mapped Group:");
                        missing.Aggregate(errors, (result, name) => result.AppendLine(name));
                    }

                    if (errors.Length > 0)
                        //throw new Exception(errors.ToString());
                        Debug.WriteLine(errors.ToString());
                }

                // This rolls up any and all exceptions encountered and rethrows them,
                // if we're trying to go for an absolute, no exceptions matching of Regex Groups to Class Properties:
                if (enforce_exact_match && match.Groups.Count - 1 != props.Count)
                {
                    errors.AppendLine(
                        $"{MethodBase.GetCurrentMethod().Name}() "
                            + $"WARNING: Number of Matched Groups ({match.Groups.Count}) "
                            + $"does not equal the number of properties for the given class '{typeof(T).Name}'({props.Count})!  "
                            + $"Check the class type and regex pattern for errors and try again."
                    );

                    errors.AppendLine("Values Parsed Successfully:");

                    for (int groupIndex = 1; groupIndex < match.Groups.Count; groupIndex++)
                    {
                        errors.Append($"{match.Groups[groupIndex].Value}\t");
                    }

                    errors.AppendLine();
                    Debug.WriteLine(errors.ToString());
                    //throw new Exception(errors.ToString());
                }

                object instance = Activator.CreateInstance(typeof(T));

                foreach (var property in props)
                {
                    // Get the raw string value that was matched by the Regex for each Group that was captured:
                    string value = match
                       .Groups
                       .Cast<Group>()
                       .SingleOrDefault(group => group.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))?.Value
                       .Trim();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        property.SetValue(
                            instance,
                            value: TypeDescriptor
                                .GetConverter(property.PropertyType)
                                .ConvertFrom(value),
                            index: null
                        );
                    }
                    else if (property.CanWrite)
                    {
                        property?.SetValue(instance, value: null, index: null);
                    }
                }

                list.Add((T)instance);
                return list;
            }
            );

            return collection;
        }

        // // (Experimental)
        // public static List<T> Extract<T>(
        //     this string text,
        //     Dictionary<Expression<Func<T, object>>, string> patterns
        // )
        // {
        //     var pattern_strings = patterns
        //         .Aggregate(new List<string>(), (current, kvp) =>
        //         {
        //             var property_name = MemberExtensions.GetMemberName(kvp.Key).Dump("propertyname");
        //             var raw_pattern = kvp.Value;

        //             string pattern_segment = string.IsNullOrWhiteSpace(raw_pattern) || !raw_pattern.Contains("?<")
        //                 ? $@"(?<{property_name}>{raw_pattern})"
        //                 : raw_pattern;

        //             current.Add(pattern_segment);
        //             return current;
        //         });

        //     var possible_patterns = pattern_strings
        //         .ToArray().GetPermutations().Dump("possible patterns");

        //     string built_pattern = pattern_strings
        //         .Aggregate(new StringBuilder()
        //         , (sb, next) => sb.Append(next)).ToString();

        //     built_pattern.Dump("built_pattern");

        //     var options =
        //             RegexOptions.Compiled
        //             | RegexOptions.IgnoreCase
        //             | RegexOptions.ExplicitCapture
        //             | RegexOptions.Multiline
        //             | RegexOptions.IgnorePatternWhitespace;

        //     return Extract<T>(text, built_pattern, options: options);
        // }


    }
}
