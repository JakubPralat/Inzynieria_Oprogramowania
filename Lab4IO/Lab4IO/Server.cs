using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab4IO
{
    class Server
    {
        private TcpListener server;
        private int port = 2048;
        private IPAddress address = IPAddress.Any;
        private bool running = false;
        private Task serverTask;

        public int Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        public Task ServerTask
        {
            get
            {
                return serverTask;
            }

            set
            {
                serverTask = value;
            }
        }

        #region constructors
        public Server()
        {
            server = new TcpListener(address, Port);
        }
        public Server(int port, string address)
        {
            this.address = IPAddress.Parse(address);
            this.Port = port;
            server = new TcpListener(this.address, this.Port);
        }
        public Server(int port, IPAddress address)
        {
            this.address = address;
            this.Port = port;
            server = new TcpListener(this.address, this.Port);
        }
        #endregion

        public void Run()
        {
            serverTask = runAsync();
        }

        async Task runAsync()
        {
            try
            {
                server.Start();
                running = true;
            }
            catch
            {
                throw new Exception("todo");
            }
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                byte[] buffer = new byte[1024];
                await client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                    async (t) =>
                    {
                        int i = t.Result;
                        while (true)
                        {
                            client.GetStream().WriteAsync(buffer, 0, i);
                            i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                        }
                    });
            }
        }
    }
    }
}
