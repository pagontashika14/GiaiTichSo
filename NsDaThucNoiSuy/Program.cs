using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace NsDaThucNoiSuy
{
    class Program
    {
        static void Main(string[] args)
        {
            Function.ConsoleTiengViet();
            var noiSuy = new DaThucNoiSuy(@"DaThucNoiSuy\newton.txt");
            Console.WriteLine("Bảng số:");
            Console.WriteLine(noiSuy.ToString());
            Console.WriteLine("--------- Lagrange");
            Console.WriteLine(noiSuy.Lagrange().ToString());
            Console.WriteLine("--------- Newton");
            Console.WriteLine(noiSuy.NewtonTongQuat().ToString());

            Console.ReadKey();
        }
    }
}
