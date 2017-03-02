using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPPProjectServer {
    class Server {
        TcpListener Listener;

        public Server(int Port) {
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();

            while (true) {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());
            }
        }

        ~Server() {
            if (Listener != null) {
                Listener.Stop();
            }
        }

        static void ClientThread(Object StateInfo) {
            new Client((TcpClient)StateInfo);
        }
    }
}
