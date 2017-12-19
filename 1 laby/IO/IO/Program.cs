using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace IO
{
    class Program
    {
        static void Main(string[] args)
        {
            //Zadanie1();
            Zadanie2();
        }

        static void Zadanie1()
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, 500);
            ThreadPool.QueueUserWorkItem(ThreadProc, 1000);

            Thread.Sleep(10000);
        }


        static void ThreadProc(Object stateInfo)
        {
            int time = ((int)stateInfo);
            Thread.Sleep(time);
            Console.WriteLine(time);
        }

        static void Zadanie2()
        {
            ThreadPool.QueueUserWorkItem(Server);
            ThreadPool.QueueUserWorkItem(Client);
            ThreadPool.QueueUserWorkItem(Client);

            Thread.Sleep(10000);
        }

        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }


        static void Server(Object stateInfo)
        {

            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                if (client.Connected)
                {
                    ThreadPool.QueueUserWorkItem(connection, new object[] { client });

                }

            }
        }

        static void connection(Object clientCon)
        {
            TcpClient client = new TcpClient();

            byte[] buffer = new byte[32];
            client.GetStream().Read(buffer, 0, 32);

            String msg = Encoding.Default.GetString(buffer);

            writeConsoleMessage(msg, ConsoleColor.Green);

            buffer = new ASCIIEncoding().GetBytes("Received!");

            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();

            Thread.Sleep(5000);

        }



        static void Client(Object stateInfo)
        {
            String messageString = "Wiadomosc";
            TcpClient client = new TcpClient();

            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new byte[32];

            message = new ASCIIEncoding().GetBytes(messageString);
            client.GetStream().Write(message, 0, message.Length);

            byte[] receiverMes = new byte[32];
            NetworkStream stream = client.GetStream();
            stream.Read(receiverMes, 0, 32);

            String received = Encoding.Default.GetString(receiverMes);

            writeConsoleMessage(received, ConsoleColor.Red);

        }






    }
}
