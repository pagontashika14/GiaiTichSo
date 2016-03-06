using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Function
    {
        public static int ChuSoTinTuong(double dk)
        {
            int i = 0;
            while (dk < 0.5 * Math.Pow(10, -i))
            {
                i++;
            }
            return i;
        }
        public static void ConsoleTiengViet()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
        public static List<double> KhoiTaoList(int length)
        {
            var list = new List<double>();
            for (int i = 0; i < length; i++)
            {
                list.Add(0);
            }
            return list;
        }
    }
}
