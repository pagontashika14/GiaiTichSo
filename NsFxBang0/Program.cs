using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NsFxBang0
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Equation equa = new Equation((ref double f, double x) =>
            {
                f = Math.Pow(x, 5) - x - 1;
            });
            Console.WriteLine("Nhập khoảng ly nghiệm:");
            Console.Write("a = "); double a = double.Parse(Console.ReadLine());
            Console.Write("\nb = "); double b = double.Parse(Console.ReadLine());
            Console.Write("\nSai số tuyệt đối: "); double epsilon = double.Parse(Console.ReadLine());
            double nghiem = 0;
            bool boo = equa.PhuongPhapChiaDoi(a, b, epsilon, ref nghiem);
            if (boo)
            {
                Console.WriteLine("Nghiệm xấp xỉ: {0}", nghiem);
            }
            Console.ReadKey();
        }
    }
}
