using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


namespace RestENATest
{
    class Program
    {
        static void Main(string[] args)
        {

            Test();
            Console.ReadKey();

        }

        /// <summary>
        /// Funcion para hacer el request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpWebResponse HttpPost(String url, String method, string data)
        {

            HttpWebResponse returnValue = null;

            try
            {
                var req = WebRequest.Create(url) as HttpWebRequest;

                req.Method = method;

                if (method.Equals("POST") && data != null)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(data);

                    var postData = req.GetRequestStream();

                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                returnValue = req.GetResponse() as HttpWebResponse;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return returnValue;

        }

        /// <summary>
        /// Prueba
        /// </summary>
        private static void Test()
        {
            var data = new StringBuilder();

            data.Append("<message>");
            data.Append("<header>");
            data.Append("<msg_type>SCD01</msg_type>");
            data.Append("<id_trx_rec>123456</id_trx_rec>");
            data.Append("<company_id>EP</company_id>");
            data.Append("<collector_id>1234567890</collector_id>");
            data.Append("<date>2014-04-06T10:23:20-04:00</date>");
            data.Append("<channel>INT</channel>");
            data.Append("<country_code>PA</country_code>");
            data.Append("<currency_code>USD</currency_code>");
            data.Append("</header>");
            data.Append("<body>");
            data.Append("<service_id>PANAPAS</service_id>");
            data.Append("<client_id/>");
            data.Append("<document_id/>");
            data.Append("<contract_id>1234</contract_id>");
            data.Append("</body>");
            data.Append("</message>");

            var ret = HttpPost("http://200.75.7.243/WSDebtRequest/webresources/monitorcall", "POST", data.ToString());

            string responseData = string.Empty;

            try
            {
                using (var stream = ret.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    responseData = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            Console.WriteLine(responseData);

        }

    }
}
