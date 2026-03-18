using System.ComponentModel;
using System.Data;

namespace Libri.BAL.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data) 
        { 
            var properties = TypeDescriptor.GetProperties(typeof(T));
            
            var dataTable = new DataTable();

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var type = property.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type)!;
                }

                dataTable.Columns.Add(property.Name, type);
            }

            var values = new object[properties.Count];
            
            foreach (var iListItem in data)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(iListItem) ?? DBNull.Value;
                }

                dataTable.Rows.Add(values);
            }

            return dataTable; 
        }
    }
}
