using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab4IO
{
    class Client
    {
        private TcpClient client;
   
        public void Connect()
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 2048);
        }
        
        public async Task<string> Ping(string message)
        {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            client.GetStream().WriteAsync(buffer, 0, buffer.Length);
            buffer = new byte[1024];
            var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, t);
        }
        
        public async Task<IEnumerable<string>> keepPinging(string message, CancellationToken token)
        {
            List<string> messagesList = new List<string>();
            bool ended = false;
            while (!ended)
            {
                if (token.IsCancellationRequested)
                    ended = true;
                messagesList.Add(await Ping(message));
            }
            return messagesList;
        }
        
    }
}
