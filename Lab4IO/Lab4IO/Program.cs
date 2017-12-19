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
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Task.WaitAll(s.ServerTask);

        }
    }
}
