using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPPProjectServer {
    class Client {
        public Client(TcpClient Client) {
            Console.WriteLine("Client connected!");
            string result = CurrencyConverter.getConvertedCurrency("UAH", "EUR", "10");
            string Html = "<html><body><h1>" + result + "</h1></body></html>";
            string Str = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            Client.Close();
            Console.WriteLine("Client disconnected!");
        }
    }
}
