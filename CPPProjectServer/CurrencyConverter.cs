using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CPPProjectServer
{
    class CurrencyConverter
    {
        public static string getConvertedCurrency(string fromCurrency, string toCurrency, string amount)
        {
            WebClient web = new WebClient();
            string url = string.Format("https://www.google.com/finance/converter?a={2}&from={0}&to={1}",
                                        fromCurrency.ToUpper(),
                                        toCurrency.ToUpper(), amount);

            string response = web.DownloadString(url);
            //Task<string> task = web.DownloadStringTaskAsync(url);
            //task.Wait();
            //string response = task.Result;
            var split = response.Split((new string[] { "<span class=bld>" }), StringSplitOptions.None);
            var value = split[1].Split(' ')[0];
            decimal rate = decimal.Parse(value, CultureInfo.InvariantCulture);
            Console.WriteLine(rate.ToString());
            //            return response;
            return rate.ToString();
        }
    }
}
