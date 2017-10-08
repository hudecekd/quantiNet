using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Quanti.Utils.Serialization.Xml
{
    /// <summary>
    /// Represents serializer which creates concreate instances from AMQP payloads or creates payload from instances.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Deserializes payload co concreate type. UTF-8 is used by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] payload) where T : class
        {
            var payloadStr = Encoding.UTF8.GetString(payload);
            return Deserialize<T>(payloadStr);
        }

        /// <summary>
        /// Deserializes payload to concreate type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string payload) where T : class
        {
            var xml = new XmlDocument();
            xml.LoadXml(payload);

            if (xml.DocumentElement.Name != typeof(T).Name) throw new InvalidOperationException(string.Format("Payload cannot be deserialized to the type '{0}'", typeof(T).Name));

            var serializer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(payload))
            {
                var obj = serializer.Deserialize(sr);
                return (T)obj;
            }
        }

        /// <summary>
        /// Checks whether payload can be deserialized to the type specified in <typeparamref name="T"/>.
        /// Name of type is checked with name of the root element in Xml which is stored in <paramref name="payload"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static bool CanDeserialize<T>(string payload) where T : class
        {
            var xml = new XmlDocument();
            xml.LoadXml(payload);

            return (xml.DocumentElement.Name == typeof(T).Name);
        }

        /// <summary>
        /// Serializes instance to byte array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T value) where T : class
        {
            byte[] buffer;
            var s = new XmlSerializer(value.GetType()); // we want exact type not some base type or "object" type

            using (var ms = new System.IO.MemoryStream())
            using (var xmlWriter = XmlWriter.Create(ms, new XmlWriterSettings()
            {
                Indent = true
            }))
            {
                s.Serialize(ms, value);
                buffer = ms.ToArray();
            }
            return buffer;
        }

        /// <summary>
        /// Serializes instance to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeToString<T>(T value) where T : class
        {
            var s = new XmlSerializer(value.GetType()); // we want exact type not some base type or "object" type
            using (var sw = new StringWriter())
            {
                s.Serialize(sw, value);
                return sw.ToString();
            }
        }
    }
}
