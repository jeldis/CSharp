using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AsyncCallSql
{
    public static class Helper
    {
        public static string GenerateHash(string message)
        {
            var encode = new ASCIIEncoding();
            var sourceByte = encode.GetBytes(message.Trim());
            var sha1 = new SHA1CryptoServiceProvider();
            var hashString = sha1.ComputeHash(sourceByte);
            return BitConverter.ToString(hashString).Replace("-", "").ToLower();
        }

        public static string Serialize<T>(IEnumerable<T> t)
        {
            using (var sw = new StringWriter())
            using (var xw = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = false }))
            {
                T[] tmp = t.ToArray();
                var xs = new XmlSerializer(tmp.GetType(), new XmlRootAttribute(typeof(T).Name));
                var ns = new XmlSerializerNamespaces();
                ns.Add(String.Empty, String.Empty);
                xs.Serialize(xw, tmp, ns);
                return sw.ToString();
            }
        }

        public static string Serialize1<T>(T t)
        {
            using (var sw = new StringWriter())
            using (var xw = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = false }))
            {

                var xs = new XmlSerializer(t.GetType(), new XmlRootAttribute(typeof(T).Name));
                var ns = new XmlSerializerNamespaces();
                ns.Add(String.Empty, String.Empty);
                xs.Serialize(xw, t, ns);
                return sw.ToString();
            }
        }

  
        public static T Deserialize<T>(string xmlString)
        {
            if (string.IsNullOrWhiteSpace(xmlString))
                return default(T);

            using (var memStream = new MemoryStream(Encoding.Unicode.GetBytes(xmlString)))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(memStream);
            }

        }

    }
}