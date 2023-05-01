using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CodeMechanic.Extensions
{
    public static class ReflectionExtensions
    {
        public static ICollection<PropertyInfo> TryGetProperties<T>(
         this IDictionary<Type, ICollection<PropertyInfo>> property_cache
            , bool ignore_case = true
            , BindingFlags flags = BindingFlags.Default
            , params string[] exclusions
        )
        {
            ICollection<PropertyInfo> properties;
            var objType = typeof(T);

            lock (property_cache)
            {
                if (!property_cache.TryGetValue(objType, out properties))
                {
                    $"Prop not found for {objType.Name} so running reflection".Dump();

                    var type_props = objType.GetProperties();

                    var lowercased_prop_names = type_props.Select(p => p.Name.ToLowerInvariant());

                    var joined_names = lowercased_prop_names.Except(exclusions);

                    properties = type_props
                        .Where(
                            property => property.CanWrite
                            && joined_names.Contains(property.Name.ToLowerInvariant())
                        )
                        .ToList();

                    property_cache.Add(objType, properties);

                    property_cache.Count.Dump("propcache size");
                }
            }

            return properties;
        }

        public static object GetPropertyValue<T>(this T self, string propertyName)
        {
            Type type = self.GetType();
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            return property.GetValue(self, null);
        }
    }

    public class PropertyModel<T> : AbstractPropertyModel
    {
        public static int StaticProperty { get; set; }
        public override int OverrideProperty { get; set; }
        public virtual int VirtualProperty { get; set; }
        internal int InternalProperty { get; set; }
        private int PrivateProperty { get; set; }
        protected int ProtectedProperty { get; set; }
        protected internal int ProtectedInternalProperty { get; set; }
        public int PublicProperty { get; set; }
        public int PublicGetterPrivateSetterProperty { get; private set; }
        public int PrivateGetterPublicSetterProperty { private get; set; }
        public T GenericProperty { get; set; }
        public T this[T param1, int param2] { get { return param1; }}
        public override int AbstractProperty { get; set; }
    }

    public abstract class AbstractPropertyModel
    {
        public abstract int AbstractProperty { get; set; }
        public virtual int OverrideProperty { get; set; }
    }
}
