using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lab3IO
{
    class Program
    {
        static void Main(string[] args)
        {
            ZadaniaTAP zadania = new ZadaniaTAP();
            Task<XmlDocument> task = zadania.Zadanie3("http://www.feedforall.com/sample.xml");
            Task.WhenAll(task);
            XmlDocument xmlDoc = task.GetAwaiter().GetResult();
        }

    }
}
