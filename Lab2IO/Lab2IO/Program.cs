using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2IO
{
    class Program
    {
        static void myAsyncCallback(IAsyncResult state)
        {
            object[] data = (object[])state.AsyncState;
            FileStream fs = (FileStream)data[0];
            AutoResetEvent are = (AutoResetEvent)data[2];
            byte[] buffer = (byte[])data[1];
            int len = fs.EndRead(state);
            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, len));
            fs.Close();
            are.Set();
        }

        static void zadanie6()
        {
            byte[] buffer = new byte[1024];
            AutoResetEvent are = new AutoResetEvent(false);
            FileStream fs = new FileStream(@"C:\Users\Kuba\Documents\Visual Studio 2015\Projects\Lab2IO\Lab2IO\Zaszyfrowany.txt", System.IO.FileMode.Open);
            fs.BeginRead(buffer, 0, buffer.Length, myAsyncCallback, new object[] { fs, buffer, are });
            are.WaitOne();
        }

        static void zadanie7()
        {
            byte[] buffer = new byte[1024];
            FileStream fs = new FileStream(@"C:\Users\Kuba\Documents\Visual Studio 2015\Projects\Lab2IO\Lab2IO\Zaszyfrowany.txt", System.IO.FileMode.Open);

            IAsyncResult state = fs.BeginRead(buffer, 0, buffer.Length, null, null);
            int len = fs.EndRead(state);
            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, len));
            fs.Close();
        }

        //ZADANIE 8

        delegate int DelegateType(int argument);
        static DelegateType rSilniaDelegate, iSilniaDelegate,
            rFibonacciDelegate, iFibonacciDelegate;

        static void Main(string[] args)
        {
            rSilniaDelegate = new DelegateType(rSilnia);
            iSilniaDelegate = new DelegateType(iSilnia);
            rFibonacciDelegate = new DelegateType(rFibonacci);
            iFibonacciDelegate = new DelegateType(iFibonacci);

            IAsyncResult rSilniaResult = rSilniaDelegate.BeginInvoke(4, null, null);
            IAsyncResult iSilniaResult = iSilniaDelegate.BeginInvoke(4, null, null);
            IAsyncResult rFibonacciResult = rFibonacciDelegate.BeginInvoke(4, null, null);
            IAsyncResult iFibonacciResult = iFibonacciDelegate.BeginInvoke(4, null, null);

            WaitHandle[] waitHandles = new WaitHandle[] {rSilniaResult.AsyncWaitHandle, iSilniaResult.AsyncWaitHandle,
            rFibonacciResult.AsyncWaitHandle, iFibonacciResult.AsyncWaitHandle};

            WaitHandle.WaitAll(waitHandles);

            int rSilniaValue = rSilniaDelegate.EndInvoke(rSilniaResult);
            int iSilniaValue = iSilniaDelegate.EndInvoke(iSilniaResult);
            int rFibonacciValue = rFibonacciDelegate.EndInvoke(rFibonacciResult);
            int iFibonacciValue = iFibonacciDelegate.EndInvoke(iFibonacciResult);

            foreach (WaitHandle temp in waitHandles)
            {
                temp.Close();
            }

            Console.WriteLine("Rekurencyjnie silnia z 4: " + rSilniaValue + "\nIteracyjnie silnia z 4: " + iSilniaValue + "\nRekurencyjnie Fibonacci z 4: "
                + rFibonacciValue + "\n" + "Iteracyjnie Fibonacci z 4: " + iFibonacciValue);

        }



        static int rSilnia(int x)
        {
            if (x < 0) throw new Exception();
            if (x <= 1) return 1;
            return x * rSilnia(x - 1);
        }

        static int iSilnia(int x)
        {
            int total = 1;
            if (x < 0) throw new Exception();
            if (x <= 1) return total;
            for (int i = 2; i <= x; i++)
                total *= i;
            return total;

        }

        public static int rFibonacci(int position)
        {
            if (position == 0)
                return 0;
            if (position == 1)
                return 1;
            else
                return rFibonacci(position - 2) + rFibonacci(position - 1);
        }

        public static int iFibonacci(int state)
        {
            int a = 0;
            int b = 1;

            for (int i = 0; i < state; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }
    }
}
