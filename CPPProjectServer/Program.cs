using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPPProjectServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int MaxThreadsCount = Environment.ProcessorCount * 2;
            ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
            ThreadPool.SetMinThreads(2, 2);

            new Server(881);
        }
    }
}
