using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NsHePhuongTrinh
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var he = new HePhuongTrinh(@"HePhuongTrinh\Gauss\he.txt");
            //he.Gauss();
            Console.WriteLine(he.ToString());
            Console.WriteLine("--------- Phương pháp Gauss ----------");
            Console.WriteLine(he.Gauss().ToString());
            Console.WriteLine("--------- Phương pháp Gauss-Jordan ---");
            Console.WriteLine(he.GaussJordan().ToString());
            Console.ReadKey();
        }
    }
}
