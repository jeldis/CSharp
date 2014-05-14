using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SerializeTest
{
    public class Helper
    {
        public static string Serialize<T>(T item)
        {
            var memStream = new MemoryStream();
            XmlTextWriter textWriter;

            using (textWriter = new XmlTextWriter(memStream, Encoding.Unicode))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(textWriter, item);

                memStream = textWriter.BaseStream as MemoryStream;
            }

            return memStream != null ? Encoding.Unicode.GetString(memStream.ToArray()) : null;

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

        public static string GenerateHash(string message)
        {
            var encode = new ASCIIEncoding();
            var sourceByte = encode.GetBytes(message.Trim());
            var sha1 = new SHA1CryptoServiceProvider();
            var hashString = sha1.ComputeHash(sourceByte);
            return BitConverter.ToString(hashString).Replace("-", "").ToLower();
        }
    }
}
