using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPProjectServer
{
    class DataParser
    {
        public static Dictionary<string, string> parseRequest(string requestURI)
        {
            if (requestURI.EndsWith(".ico")) {
                return new Dictionary<string, string>();
            } else
            {
                string[] result = requestURI.Split('_');
                if(result.Count() != 6)
                {
                    return new Dictionary<string, string>();
                }

                var resultDictionary = new Dictionary<string, string>();

                resultDictionary.Add("fromWeight", result[0]);
                resultDictionary.Add("toWeight", result[1]);
                resultDictionary.Add("fromWeightNumber", result[2]);
                resultDictionary.Add("fromCurrency", result[3]);
                resultDictionary.Add("toCurrency", result[4]);
                resultDictionary.Add("fromCurrencyNumber", result[5]);

                return resultDictionary;
            }
        }
    }
}
