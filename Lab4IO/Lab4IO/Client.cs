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
        private Task clientTask;
        private int port;
        private IPAddress address;
        private bool running = false;
        public Client()
        {
            client = new TcpClient();
        }

        public Task ClientTask
        {
            get
            {
                return clientTask;
            }

            set
            {
                this.clientTask = value;
            }
        }

        public bool Running
        {
            get
            {
                return running;
            }
        }
        public IPAddress Address
        {
            get
            {
                return address;
            }
            set
            {
                if (!running) address = value;
                else throw new Exception("Proba zmiany adresu IP w trakcie dzialania serwera.");
            }
            }

        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                if (!running) port = value;
                else throw new Exception("Proba zmiany portu w trakcie dzialania serwera.");
            }
            }

        public void Run()
        {
            clientTask = runAsync();
        }

        async Task runAsync()
        {
            try
            {
                client.Start();
                running = true;
            }
            catch
            {
                throw new Exception("todo");
            }
            while (true)
            {
                TcpClient client = await client.AcceptTcpClientAsync();
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
