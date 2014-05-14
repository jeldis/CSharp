using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace WcfServiceQueryStringTest
{

    public class Service1 : IService1
    {
        /// <summary>
        /// http://localhost:54132/Service1.svc?monto=888
        /// https://gist.github.com/sanjayuttam/381276
        /// </summary>
        /// <param name="value"></param>
        /// <param name="monto"></param>
        /// <returns></returns>
        public string GetData(int value)
        {

            string monto = string.Empty;

            if (WebOperationContext.Current != null)
            {

                monto = GetQueryString("monto");

            }
            return string.Format("You entered: {0} and amount: {1}", value, monto);
        }

        //https://gist.github.com/sanjayuttam/381276
        private string GetQueryString(string key)
        {
            if (OperationContext.Current == null) return string.Empty;
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            var requestProperty = properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;

            if (requestProperty != null)
            {
                string queryString = requestProperty.QueryString;
                if (!queryString.Contains(key)) return string.Empty;
                return HttpUtility.ParseQueryString(queryString)[key];
            }
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
