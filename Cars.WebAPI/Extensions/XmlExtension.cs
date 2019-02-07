using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Cars.WebAPI.Extensions
{
    internal static class XmlExtension
    {
        internal static string ObjectToXml<T>(T obj)
        {            
            var xml = string.Empty;

            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
                    xsSubmit.Serialize(writer, obj);
                    xml = sw.ToString();
                }
            };

            return xml;
        }
    }
}
