using Libri.BAL.Helpers;
using System.Xml;
using System.Xml.Serialization;

namespace Libri.BAL.Extensions
{
    public static class XmlExtensions
    {
        public static string ToXml<T>(this IEnumerable<T> listObj)
        {
            if (listObj == null || !listObj.Any())
            {
                throw new ArgumentNullException(nameof(listObj), "Dữ liệu không được để trống.");
            }

            if (!typeof(T).IsSerializable)
            {
                throw new InvalidOperationException($"Không thể chuyển đổi dữ liệu của {typeof(T).FullName}.");
            }

            var rootName = XmlPluralization.Pluralize(typeof(T).Name);
            XmlSerializer xmlSerializer = new(typeof(List<T>), new XmlRootAttribute(rootName));

            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8
            };

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            xmlSerializer.Serialize(xmlWriter, listObj);
            
            return stringWriter.ToString();
        }
        
        public static string SingleObjectToXml<T>(this T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Dữ liệu không được để trống.");
            }

            if (!typeof(T).IsSerializable)
            {
                throw new InvalidOperationException($"Không thể chuyển đổi dữ liệu của {typeof(T).FullName}.");
            }

            XmlSerializer xmlSerializer = new(typeof(T));

            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8
            };

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            xmlSerializer.Serialize(xmlWriter, obj);
            
            return stringWriter.ToString();
        }

    }
}
