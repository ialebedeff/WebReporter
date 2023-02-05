using System.Data.Common;
using System.Reflection;
using System.Text.Json;

namespace AutoConverter
{
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public string ColumnName { get; set; }
    }
    public class RecordsConverter
    {
        public static T ConvertTo<T>(Dictionary<string, object>? value)
            where T : class
        {
            var instance = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();

            if (value is null)
            { 
                return instance;
            }

            foreach (var item in properties.Select((Property, Index) => (Property, Index)))
            {
                var attr = item.Property.GetCustomAttribute<ColumnAttribute>();

                if (attr != null)
                {
                    try
                    {
                        item.Property.SetValue(instance, ((JsonElement)value[attr.ColumnName]).
                            Deserialize(item.Property.PropertyType), null);
                    }
                    catch { }
                }
            }

            return instance;
        }

        public static IEnumerable<T> ConvertTo<T>(List<Dictionary<string, object>> values)
            where T : class
        {
            if (values is null)
            {
                return Enumerable.Empty<T>();
            }

            var valuesList = new List<T>();

            foreach (var value in values)
            {
                valuesList.Add(ConvertTo<T>(value));
            }

            return valuesList;
        }
    }
}