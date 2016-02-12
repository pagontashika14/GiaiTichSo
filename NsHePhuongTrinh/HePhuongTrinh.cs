using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NsMaTran;
using Common;

namespace NsHePhuongTrinh
{
    class HePhuongTrinh
    {
        public HePhuongTrinh()
        {
            Console.WriteLine("");
        }
        public HePhuongTrinh(string file)
        {
            var txt = new ReadTxt(file);
            var data = txt.TachPhanTuSo();
            dataToHPT(data);
        }
        #region Member
        MaTran MT;
        #endregion
        #region Public Method
        public Nghiem Gauss()
        {
            var mt = MT.DuaVeMaTranBacThang();
            var sdk0 = mt.SoDongKhacKhong;
            var sdk0A = mt.SoDongKhacKhongA;
            Nghiem nghiem;
            if (sdk0 > sdk0A)
            {
                // Vo nghiem
                nghiem = new Nghiem(0, 0);
                return nghiem;
            }
            if (sdk0 == sdk0A && sdk0 == mt.N)
            {
                // Co nghiem duy nhat
                nghiem = new Nghiem(Status.NghiemDuyNhat, sdk0);
                for (int i = sdk0 - 1; i >= 0; i--)
                {
                    nghiem[i,0] = mt.MTGhep[i, 0];
                    for (int j = 0; j < mt.N; j++)
                    {
                        if (mt[i, j] == 0 || j == i)
                        {
                            continue;
                        }
                        nghiem[i,0] -= mt[i, j] * nghiem[j,0];
                    }
                    nghiem[i,0] /= mt[i, i];
                }
                return nghiem;
            }
            if (sdk0 == sdk0A && sdk0 < mt.N)
            {
                // Co vo so nghiem
                nghiem = new Nghiem(Status.VoSoNghiem, mt.N);
                int dongChon = 0;
                for (int i = 0; i < mt.N; i++)
                {
                    if (mt[dongChon, i] != 0)
                    {
                        nghiem.Loai[i] = LoaiNghiem.PhuThuoc;
                        for (int j = dongChon; j < mt.N; j++)
                        {
                            if (j == i)
                            {
                                continue;
                            }
                            nghiem[i, j] = -mt[dongChon, j] / mt[dongChon, i];
                        }
                        nghiem[i, mt.N] = mt.MTGhep[dongChon, 0] / mt[dongChon, i];
                        dongChon++;
                        if (dongChon > mt.M - 1)
                        {
                            break;
                        }
                    }
                }
                return nghiem;
            }

            return new Nghiem(0, 0);
        }
        public Nghiem GaussJordan()
        {
            var mt = MT.Copy();
            int i = 0, j = 0;
            var luuHang = new Dictionary<int, int>();
            for (int m = 0; m < mt.M; m++)
            {
                mt.TimPhanTuTroi(luuHang, ref i, ref j);
                if (mt[i, j] == 0)
                {
                    break;
                }
                luuHang[i] = j;
                for (int k = 0; k < mt.M; k++)
                {
                    if (k == i)
                    {
                        continue;
                    }
                    mt.BienDoiTrenHang(k, i, (ref double ak, ref double ai) =>
                    {
                        ak = ak - ai * mt[k, j] / mt[i, j];
                    });
                }
            }
            // Biện luận
            var sdk0 = mt.SoDongKhacKhong;
            var sdk0A = mt.SoDongKhacKhongA;
            Nghiem nghiem;
            if (sdk0 > sdk0A)
            {
                // Vo nghiem
                nghiem = new Nghiem(0, 0);
                return nghiem;
            }
            if (sdk0 == sdk0A && sdk0 == mt.N)
            {
                // Co nghiem duy nhat
                nghiem = new Nghiem(Status.NghiemDuyNhat, sdk0);
                foreach (KeyValuePair<int, int> item in luuHang)
                {
                    nghiem[item.Value,0] = mt.MTGhep[item.Key, 0] / mt[item.Key, item.Value];
                }
                return nghiem;
            }
            if (sdk0 == sdk0A && sdk0 < mt.N)
            {
                // Co vo so nghiem
                nghiem = new Nghiem(Status.VoSoNghiem, mt.N);
                foreach (KeyValuePair<int, int> item in luuHang)
                {
                    nghiem.Loai[item.Value] = LoaiNghiem.PhuThuoc;
                    for (int k = 0; k < mt.N; k++)
                    {
                        if (k == item.Value)
                        {
                            continue;
                        }
                        nghiem[item.Value, k] = -mt[item.Key, k];
                    }
                    nghiem[item.Value, mt.N] = mt.MTGhep[item.Key, 0];
                }
                return nghiem;
            }
            return new Nghiem(0, 0);
        }
        public Nghiem LapDon(double dkDung)
        {
            var mt = MT.Copy();
            if (mt.M != mt.N)
            {
                throw new Exception("Không phải ma trận vuông");
            }
            if (!mt.isMaTranCheoTroi)
            {
                throw new Exception("Không phải ma trận chéo trội");
            }
            var a = new MaTran(mt.M, mt.N);
            var b = new MaTran(mt.M, 1);
            for (int i = 0; i < a.M; i++)
            {
                for (int j = 0; j < a.N; j++)
                {
                    if (j == i)
                    {
                        a[i, j] = 0;
                        continue;
                    }
                    a[i, j] = -mt[i, j] / mt[i, i];
                }
                b[i, 0] = mt.MTGhep[i, 0] / mt[i, i];
            }
            var cstt = Function.ChuSoTinTuong(dkDung);
            dkDung = dkDung - 0.5 * Math.Pow(10, -cstt);
            double cvc = a.ChuanVoCung;
            var x = new Nghiem(Status.NghiemDuyNhat, mt.M);
            var x0 = x.Copy();
            var x1 = a * x0 + b;
            var soPhepLap =Math.Ceiling(
                Math.Log(dkDung * (1 - cvc) / (x1 - x0).ChuanVoCung) 
                / 
                Math.Log(cvc)
                );
            for (int i = 0; i < soPhepLap; i++)
            {
                x = new Nghiem(a * x1 + b);
                x1 = x.Copy();
            }
            for (int i = 0; i < x.M; i++)
            {
                x[i, 0] = Math.Round(x[i, 0], cstt);
            }
            return x;
        }
        public override string ToString()
        {
            string s = "Hệ :\n";
            for (int i = 0; i < MT.M; i++)
            {
                s += string.Format("{0,4}.x[0] ", MT[i, 0]);
                for (int j = 1; j < MT.N; j++)
                {
                    if (MT[i, j] >= 0)
                    {
                        s += string.Format("+ {0,4}.x[{1}] ", MT[i, j], j);
                    }
                    else if (MT[i, j] < 0)
                    {
                        s += string.Format("- {0,4}.x[{1}] ", -MT[i, j], j);
                    }
                }
                s += string.Format("= {0,4}\n", MT.MTGhep[i, 0]);
            }
            return s;
        }
        
        #endregion
        #region Privare Method
        void dataToHPT(List<List<double>> data)
        {
            var A = new List<List<double>>();
            var B = new List<List<double>>();
            foreach (var line in data)
            {
                var lineA = new List<double>();
                var lineB = new List<double>();
                for (int i = 0; i < line.Count; i++)
                {
                    if (i == line.Count - 1)
                    {
                        lineB.Add(line[i]);
                    }
                    else
                    {
                        lineA.Add(line[i]);
                    }
                }
                A.Add(lineA);
                B.Add(lineB);
            }
            MT = new MaTran(A);
            MT.MTGhep = new MaTran(B);
        }
        #endregion
    }
}
