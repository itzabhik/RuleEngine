using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RulesEngine.RuleEngineMetadata
{
    static class RuleAwareEntityPropertyInfo
    {
        private static Dictionary<string, Dictionary<string, Property>> _dynamicProperties
           = new Dictionary<string, Dictionary<string, Property>>();

        private static Dictionary<string, Dictionary<string, Property>> _calculatedProperties
           = new Dictionary<string, Dictionary<string, Property>>();


        public static void CreateDynamicProperty<TEnity, TType>(string entityType, string name)
        {
            CreateDynamicProperty<TEnity>(entityType, name, typeof(TType));
        }
        public static void CreateDynamicProperty<TEnity, TType>(string entityType, string name, TType defaultValue)
        {
            CreateDynamicProperty<TEnity>(entityType, name, typeof(TType), defaultValue);
        }

        public static void CreateDynamicProperty<TEnity>(string entityType, string name, Type type)
        {
            CreateDynamicProperty<TEnity>(entityType, name, type, Property.Getdefault(type));
        }

        public static void CreateDynamicProperty<TEnity>(string entityType, string name, Type type, object defaultValue)
        {

            Property prop = GetProperty(entityType, name, typeof(TEnity));
            if (prop != Property.PropertyNotFound)
            {
                throw new ArgumentException
                    (string.Format("There is already a property {0} of category {1} present in {2}", name, prop.Category, typeof(TEnity)));
            }
            if (_dynamicProperties.ContainsKey(entityType))
            {
                var propHolder = _dynamicProperties[entityType];
                TryAddProperty(name, new Property(name, type, defaultValue, PropertyCategory.Dynamic), propHolder);
            }
            else
            {
                _dynamicProperties.Add(entityType, new Dictionary<string, Property>
                {
                    {name, new Property(name,type,defaultValue,PropertyCategory.Dynamic)}
                });
            }
        }

        internal static void CreateCalculatedProperty<TEnity>(string entityType, Property calculatedProperty)
        {

            Property prop = GetProperty(entityType, calculatedProperty.Name, typeof(TEnity));
            if (prop != Property.PropertyNotFound)
            {
                if (prop.IsCalculated && prop.Type == calculatedProperty.Type )
                {
                    return;
                }

                throw new ArgumentException
                  (string.Format("There is already a property {0} of category {1} present in {2}", calculatedProperty.Name, prop.Category, typeof(TEnity)));

            }
            if (!calculatedProperty.IsCalculated)
                throw new ArgumentException("Only aggregateproperty is allowed");

            if (_calculatedProperties.ContainsKey(entityType))
            {
                var propHolder = _calculatedProperties[entityType];
                TryAddProperty(calculatedProperty.Name, calculatedProperty, propHolder);
            }
            else
            {
                _calculatedProperties.Add(entityType, new Dictionary<string, Property>
                {
                    {calculatedProperty.Name, calculatedProperty}
                });
            }

        }
        private static bool TryAddProperty(string name, Property property, Dictionary<string, Property> propHolder)
        {
            if (propHolder.ContainsKey(name))
            {
                return false;
            }
            propHolder.Add(name, property);
            return true;
        }

        internal static bool HasProperty<TEntity>(string entityType, string property)
        {
            return GetProperty(entityType, property, typeof(TEntity)) != Property.PropertyNotFound;
        }
        internal static Property GetProperty(string entityType, string property, Type instanceType)
        {
            var prop = GetDynamicProperty(entityType, property);
            if (prop != Property.PropertyNotFound)
            {
                return prop;
            }

            prop = GetCalculatedProperty(entityType, property);
            if (prop != Property.PropertyNotFound)
            {
                return prop;
            }

            if (instanceType == null)
                return Property.PropertyNotFound;

            var info = GetInstanceProperty(instanceType, property);
            if (info != Property.PropertyNotFound)
                return info;

            return Property.PropertyNotFound;
        }

        internal static Property GetInstanceProperty(Type entity, string property)
        {
            var info = GetInstancePropertyInfo(entity, property);
            if (info != null)
                return new Property(property, info.PropertyType, PropertyCategory.Instance);
            return Property.PropertyNotFound;
        }

        internal static PropertyInfo GetInstancePropertyInfo(Type entity, string property)
        {
            return entity.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
        }

        internal static Property GetDynamicProperty(string entityType, string property)
        {
            var properties = GetDynamicPropertiesForAType(entityType);
            if (properties.Count == 0)
                return Property.PropertyNotFound;
            Property serachedProp;
            properties.TryGetValue(property, out serachedProp);
            return serachedProp == null ? Property.PropertyNotFound : serachedProp;
        }

        internal static Property GetCalculatedProperty(string entityType, string property)
        {
            var properties = GetCalculatedPropertiesForAType(entityType);
            if (properties.Count == 0)
                return Property.PropertyNotFound;
            Property serachedProp;
            properties.TryGetValue(property, out serachedProp);
            return serachedProp == null ? Property.PropertyNotFound : serachedProp;
        }


        internal static Dictionary<string, Property> GetDynamicPropertiesForAType(string entityType)
        {
            if (_dynamicProperties.ContainsKey(entityType))
                return _dynamicProperties[entityType];
            return new Dictionary<string, Property>();
        }

        internal static Dictionary<string, Property> GetCalculatedPropertiesForAType(string entityType)
        {
            if (_calculatedProperties.ContainsKey(entityType))
                return _calculatedProperties[entityType];
            return new Dictionary<string, Property>();
        }
    }
}
