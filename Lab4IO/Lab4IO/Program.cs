using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab4IO
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server();
            s.Run();
            
            Client client1 = new Client();
            Client client2 = new Client();
            client1.Connect();
            client2.Connect();
            
            CancellationTokenSource cancellationToken1 = new CancellationTokenSource();
            CancellationTokenSource cancellationToken2 = new CancellationTokenSource();
            
            
            var zm1 = client.keepPinging("Wiadomosc Client1", cancellationToken1.Token);
            var zm2 = client2.keepPinging("Wiadomosc Client2", cancellationToken2.Token);
            
            cancellationToken1.CancelAfter(3000);
            cancellationToken2.CancelAfter(4000);
            Task.WaitAll(new Task[] {zm1, zm2 });
            
        }
    }
}
