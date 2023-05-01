using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMechanic.Extensions
{
    public static class DiagnosticExtensions
    {
        public static Task Sleep(int ms = 1000, Action callback = null)
        {
            return Task.Run(
                () =>
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(ms));
                    if (callback != null)
                        callback();
                }
            );
        }

        /// <summary>
        /// Outputs your object to JSON
        /// Great for cases where we can't use traditional breakpoints.
        /// Can be used "in between" calls of virtually any object and still print.
        ///
        /// Quicker than CW or Debug.
        /// Can be used inside of Lambdas (Linq), IQueryables, DataTables, etc...
        /// Hooah!
        ///
        /// Usage:
        ///     myComplexObject.Dump("Orders").doSomethingElse();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Dump<T>(
            this T obj,
            string header = null,
            bool ignoreNulls = false,
            Action<string> printFn = null,
            [CallerMemberName] string method_name = ""
        )
        {
#if DEBUG  // This segment only runs when in Debug mode (never production)

            //This Assumes it's running a console app, if no function is provided.
            if (printFn == null)
            {
                if (Debugger.IsAttached)
                {
                    printFn = (s) => Debug.WriteLine(s);
                }
                else
                {
                    printFn = Console.WriteLine;
                }
            }

            if (obj != null)
            {
                var type = obj.GetType();

                // set header of JSON if not provided
                if (string.IsNullOrWhiteSpace(header))
                    header = type.Name;

                var nested_types = type.GetNestedTypes().Select(t => t.Name);

                // The magic
                string prettyJson = JsonConvert.SerializeObject(
                    obj,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        //ContractResolver = new WritablePropertiesOnlyResolver(),
                        ContractResolver = new ExcludeCalculatedResolver(),
                        Converters = new List<JsonConverter>
                        {
                            new Newtonsoft.Json.Converters.StringEnumConverter()
                        },
                        NullValueHandling = ignoreNulls
                            ? NullValueHandling.Ignore
                            : NullValueHandling.Include,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
                string nested_types_names = new StringBuilder().AppendEach(nested_types, t => t, delimiter: ", ").ToString();
                printFn($"{method_name}()\n{header}:\n\t{nested_types_names}\n\t\t{prettyJson}");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(header))
                    printFn($"'{header}' is null.");
            }

#endif
            // return the original object.  This allows this function to be chained to (virtually) any object.
            return obj;
        }
    }

    /// <summary>
    /// Source: https://stackoverflow.com/questions/18543482/is-there-a-way-to-ignore-get-only-properties-in-json-net-without-using-jsonignor
    /// </summary>
    sealed class WritablePropertiesOnlyResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            return props.Where(p => p.Writable).ToList();
        }
    }
    sealed class ExcludeCalculatedResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = _ => ShouldSerialize(member);
            return property;
        }

        internal static bool ShouldSerialize(MemberInfo memberInfo)
        {
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo == null)
            {
                return false;
            }

            if (propertyInfo.SetMethod != null)
            {
                return true;
            }

            var getMethod = propertyInfo.GetMethod;
            return Attribute.GetCustomAttribute(getMethod, typeof(CompilerGeneratedAttribute)) != null;
        }
    }
}
