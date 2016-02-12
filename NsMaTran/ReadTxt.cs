using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace NsMaTran
{
    public class ReadTxt
    {
        public ReadTxt(string fileTxt)
        {
            string filePath = LayDuongDanChinh() + fileTxt;
            file = File.ReadAllLines(filePath, Encoding.UTF8).ToList();
        }
        #region Member
        List<string> file;
        #endregion
        #region Public Method
        public static string LayDuongDanChinh()
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            return path + @"\File\";
        }
        public override string ToString()
        {
            string strFile = "";
            foreach (string item in file)
            {
                strFile = string.Format("{0}{1}\n", strFile, item);
            }
            return strFile;
        }
        public List<List<string>> TachPhanTu()
        {
            var he = new List<List<string>>();
            foreach (string line in file)
            {
                List<string> ptu = line.Split(' ').ToList();
                for (int i = 0; i < ptu.Count; i++)
                {
                    if(ptu[i] == "")
                    {
                        ptu.RemoveAt(i);
                        i--;
                    }
                }
                he.Add(ptu);
            }
            return he;
        }
        public List<List<double>> TachPhanTuSo()
        {
            var he = new List<List<double>>();
            var listString = TachPhanTu();
            foreach (List<string> item in listString)
            {
                var pt = new List<double>();
                foreach (string ptu in item)
                {
                    double db = double.Parse(ptu);
                    pt.Add(db);
                }
                he.Add(pt);
            }
            return he;
        }
        #endregion
        #region Private Method

        #endregion
    }
}
