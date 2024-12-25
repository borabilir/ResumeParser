using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ResumeParser.API.Utils
{
    public sealed class PropertyMapper<T>
    {
        private readonly Dictionary<string, string> m_maps = new Dictionary<string, string>();
        private readonly Dictionary<string, string> m_props = new Dictionary<string, string>();

        private static string GetPropertyName<TReturn>(Expression<Func<T, TReturn>> property)
        {
            var expr = property.Body as MemberExpression;
            if (expr == null)
                throw new InvalidOperationException("Invalid property type");
            return expr.Member.Name;
        }

        public void MapProperty<TReturn>(Expression<Func<T, TReturn>> property, string mapTo)
        {
            var propName = GetPropertyName(property);
            m_maps[mapTo] = propName;
            m_props[propName] = mapTo;
        }

        public string GetPropertyMap<TReturn>(Expression<Func<T, TReturn>> property)
        {
            return GetPropertyMap(GetPropertyName(property));
        }

        public string GetPropertyMap(string property)
        {
            string result;
            return m_props.TryGetValue(property, out result) ? result : null;
        }

        public object GetPropertyValue(T instance, string property)
        {
            string result;
            if (!m_maps.TryGetValue(property, out result))
                return null;
            return typeof(T).GetProperty(result).GetValue(instance, null);
        }

        public IDictionary<string, object> GetPropertyValues(T instance)
        {
            return m_maps
                .Select(x => new { Name = x.Key, Value = typeof(T).GetProperty(x.Value).GetValue(instance, null) })
                .Where(x => x.Name != null && x.Value != null)
                .ToDictionary(x => x.Name, x => x.Value);
        }
    }
}
