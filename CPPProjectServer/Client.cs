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
            string Request = "";
            // Буфер для хранения принятых от клиента данных
            byte[] Buffer = new byte[1024];
            // Переменная для хранения количества байт, принятых от клиента
            int Count;
            // Читаем из потока клиента до тех пор, пока от него поступают данные
            while ((Count = Client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
            {
                // Преобразуем эти данные в строку и добавим ее к переменной Request
                string buffer = Encoding.ASCII.GetString(Buffer, 0, Count);
                Request += buffer;
                // Запрос должен обрываться последовательностью \r\n\r\n
                // Либо обрываем прием данных сами, если длина строки Request превышает 4 килобайта
                // Нам не нужно получать данные из POST-запроса (и т. п.), а обычный запрос
                // по идее не должен быть больше 4 килобайт
                if (Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                {
                    break;
                }
            }
            Match ReqMatch = Regex.Match(Request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");

            // Если запрос не удался
            if (ReqMatch == Match.Empty)
            {
                // Передаем клиенту ошибку 400 - неверный запрос
                SendError(Client, 400);
                return;
            }

            // Получаем строку запроса
            string RequestUri = ReqMatch.Groups[1].Value;

            // Приводим ее к изначальному виду, преобразуя экранированные символы
            // Например, "%20" -> " "
            RequestUri = Uri.UnescapeDataString(RequestUri);

            // Если в строке содержится двоеточие, передадим ошибку 400
            if (RequestUri.IndexOf("..") >= 0)
            {
                SendError(Client, 400);
                return;
            }

            // Если строка запроса оканчивается на "/", то добавим к ней index.html
            if (RequestUri.EndsWith("/"))
            {
                RequestUri += "index.html";
            }
            //   Console.WriteLine(Request);
            Console.WriteLine(RequestUri);

            Dictionary<string, string> requestDictionary = DataParser.parseRequest(RequestUri);
            if (requestDictionary.Count == 0)
            {
                SendError(Client, 400);
            }
            else
            {
                string massResult = Convert.ToString(MassConverter.convert(requestDictionary["fromWeight"], requestDictionary["toWeight"], Convert.ToDouble(requestDictionary["fromWeightNumber"])));
                string result = CurrencyConverter.getConvertedCurrency(requestDictionary["fromCurrency"], requestDictionary["toCurrency"], requestDictionary["fromCurrencyNumber"]);
                string div = Convert.ToString(Convert.ToDouble(result) / Convert.ToDouble(massResult));
                string Html = "<html><body><h1>" + massResult + " " + result + " " + div + "</h1></body></html>";
                string Str = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
                byte[] Buffer2 = Encoding.ASCII.GetBytes(Str);
                Client.GetStream().Write(Buffer2, 0, Buffer2.Length);
                Client.Close();
            }
            Console.WriteLine("Client disconnected!");
        }

        private void SendError(TcpClient Client, int Code)
        {
            // Получаем строку вида "200 OK"
            // HttpStatusCode хранит в себе все статус-коды HTTP/1.1
            string CodeStr = Code.ToString() + " " + ((HttpStatusCode)Code).ToString();
            // Код простой HTML-странички
            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            // Необходимые заголовки: ответ сервера, тип и длина содержимого. После двух пустых строк - само содержимое
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            // Приведем строку к виду массива байт
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            // Отправим его клиенту
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            // Закроем соединение
            Client.Close();
        }
    }
}
