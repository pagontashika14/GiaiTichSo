using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NsMaTran
{
    public class MaTran
    {
        public MaTran()
        {
            MT = new List<List<double>>();
        }
        public MaTran(int m, int n)
        {
            newMaTran(m, n);
        }
        public MaTran(string file)
        {
            var txt = new ReadTxt(file);
            MT = txt.TachPhanTuSo();
        }
        public MaTran(List<List<double>> mt)
        {
            MT = mt;
        }
        public MaTran(MaTran B)
        {
            newMaTran(B.M, B.N);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    this[i, j] = B[i, j];
                }
            }
            if (MTGhep != null)
            {
                MTGhep = B.MTGhep.Copy();
            }
        }
        public double this[int m, int n]
        {
            get
            {
                return MT[m][n];
            }
            set
            {
                MT[m][n] = value;
            }
        }
        public List<double> this[int m]
        {
            get
            {
                return MT[m];
            }
        }
        #region Member delegate
        public delegate void XuLyHaiHang(ref double a, ref double b);
        public delegate void XuLyHang(ref double a);
        #endregion
        #region Member
        protected List<List<double>> MT = new List<List<double>>();
        public int M { get { return MT.Count; } }
        public int N { get { return MT[0].Count; } }
        public MaTran MTGhep { get; set; }
        public int SoDongKhacKhong
        {
            get
            {
                return demSoDongKhacKhong(false);
            }
        }
        public int SoDongKhacKhongA
        {
            get
            {
                return demSoDongKhacKhong(true);
            }
        }
        public static MaTran DonVi(int length)
        {
            var mtDonVi = new MaTran(length, length);
            for (int i = 0; i < length; i++)
            {
                mtDonVi[i, i] = 1;
            }
            return mtDonVi;
        }
        public double ChuanVoCung
        {
            get
            {
                double big = 0;
                for (int i = 0; i < M; i++)
                {
                    double line = 0;
                    for (int j = 0; j < N; j++)
                    {
                        line += Math.Abs(this[i, j]);
                    }
                    if (line > big)
                    {
                        big = line;
                    }
                }
                return big;
            }
        }
        public bool isMaTranCheoTroi
        {
            get
            {
                for (int i = 0; i < M; i++)
                {
                    double line = 0;
                    for (int j = 0; j < N; j++)
                    {
                        if (j == i)
                        {
                            continue;
                        }
                        line += Math.Abs(this[i, j]);
                    }
                    if (line >= Math.Abs(this[i, i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #endregion
        #region Public Method
        public MaTran Copy()
        {
            var a = new MaTran(M, N);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    a[i, j] = this[i, j];
                }
            }
            if (MTGhep != null)
            {
                a.MTGhep = MTGhep.Copy();
            }
            return a;
        }
        public void Copy(MaTran B)
        {
            MaTran A = B.Copy();
            MT = A.MT;
            MTGhep = A.MTGhep;
        }
        public override string ToString()
        {
            string s = "";
            int temp;
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (int.TryParse(this[i, j].ToString(), out temp))
                    {
                        s = string.Format("{0}{1,-8}  ", s, this[i, j]);
                    }
                    else
                    {
                        s = string.Format("{0}{1,-8:0.000000}  ", s, this[i, j]);
                    }
                }
                if (MTGhep != null)
                {
                    s += "|  ";
                    for (int j = 0; j < MTGhep.N; j++)
                    {
                        if (int.TryParse(MTGhep[i, j].ToString(), out temp))
                        {
                            s = string.Format("{0}{1,-8}  ", s, MTGhep[i, j]);
                        }
                        else
                        {
                            s = string.Format("{0}{1,-8:0.000000}  ", s, MTGhep[i, j]);
                        }
                    }
                }
                s += "\n";
            }
            return s;
        }
        public MaTran DuaVeMaTranBacThang()
        {
            var A = this.Copy();
            int cotChon = 0;
            for (int i = 0; i < A.M; i++)
            {
                if (cotChon > A.N - 1) break;
                double bigNum = 0;
                int dong = i;
                for (int j = i; j < A.M; j++)
                {
                    if (A[j, cotChon] == 1)
                    {
                        dong = j;
                        break;
                    }
                    if (Math.Abs(A[j, cotChon]) > bigNum)
                    {
                        bigNum = Math.Abs(A[j, cotChon]);
                        dong = j;
                    }
                }
                if (A[dong, cotChon] == 0)
                {
                    cotChon++;
                    i--;
                    continue;
                }
                else
                {
                    A.BienDoiTrenHang(i, dong, (ref double a, ref double b) =>
                    {
                        double temp = a;
                        a = b;
                        b = temp;
                    });
                }
                for (int j = i + 1; j < A.M; j++)
                {
                    A.BienDoiTrenHang(i, j, (ref double ai, ref double aj) =>
                    {
                        aj = aj - ai * A[j, cotChon] / A[i, cotChon];
                    });
                }
            }
            return A;
        }
        public void TimPhanTuTroi(Dictionary<int, int> boQua, ref int i, ref int j)
        {
            double bigNum = 0;
            var boQuaHang = boQua.Keys.ToList();
            for (int m = 0; m < M; m++)
            {
                if (boQuaHang.Exists(s => s == m))
                {
                    continue;
                }
                for (int n = 0; n < N; n++)
                {
                    if (Math.Abs(MT[m][n]) == 1)
                    {
                        i = m;
                        j = n;
                        return;
                    }
                }
            }
            for (int m = 0; m < M; m++)
            {
                if (boQuaHang.Exists(s => s == m))
                {
                    continue;
                }
                for (int n = 0; n < N; n++)
                {
                    if (Math.Abs(MT[m][n]) > bigNum)
                    {
                        i = m;
                        j = n;
                        bigNum = Math.Abs(MT[m][n]);
                    }
                }
            }
        }
        public void BienDoiTrenHang(int hangA, int hangB, XuLyHaiHang xuLyHang)
        {
            MaTran B = this.Copy();
            for (int i = 0; i < B.N; i++)
            {
                double a = B[hangA, i];
                double b = B[hangB, i];
                xuLyHang(ref a, ref b);
                B[hangA, i] = a;
                B[hangB, i] = b;
            }
            if (MTGhep != null)
            {
                for (int i = 0; i < B.MTGhep.N; i++)
                {
                    double a = B.MTGhep[hangA, i];
                    double b = B.MTGhep[hangB, i];
                    xuLyHang(ref a, ref b);
                    B.MTGhep[hangA, i] = a;
                    B.MTGhep[hangB, i] = b;
                }
            }
            this.Copy(B);
        }
        public void BienDoiTrenHang(int hang, XuLyHang xuLyHang)
        {
            MaTran B = this.Copy();
            for (int i = 0; i < B.N; i++)
            {
                double a = B[hang, i];
                xuLyHang(ref a);
                B[hang, i] = a;
            }
            if (MTGhep != null)
            {
                for (int i = 0; i < B.MTGhep.N; i++)
                {
                    double a = B.MTGhep[hang, i];
                    xuLyHang(ref a);
                    B.MTGhep[hang, i] = a;
                }
            }
            this.Copy(B);
        }
        public MaTran CongVoiMaTran(MaTran B)
        {
            if (M == B.M && N == B.N)
            {
                MaTran C = new MaTran(M,N);
                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        C[i, j] = this[i, j] + B[i, j];
                    }
                }
                return C;
            }
            else
            {
                throw new Exception("Không cộng được");
            }
        }
        public MaTran NhanVoiMaTran(MaTran B)
        {
            if (N == B.M)
            {
                var C = new MaTran(M, B.N);
                for (int i = 0; i < C.M; i++)
                {
                    for (int j = 0; j < C.N; j++)
                    {
                        for (int k = 0; k < N; k++)
                        {
                            C[i, j] += this[i, k] * B[k, j];
                        }
                    }
                }
                return C;
            }
            else
            {
                throw new Exception("không nhân được");
            }
        }
        public static MaTran operator + (MaTran A,MaTran B)
        {
            return A.CongVoiMaTran(B);
        }
        public static MaTran operator * (MaTran A,MaTran B)
        {
            return A.NhanVoiMaTran(B);
        }
        public static MaTran operator - (MaTran A)
        {
            var B = A.Copy();
            for (int i = 0; i < B.M; i++)
            {
                for (int j = 0; j < B.N; j++)
                {
                    B[i, j] = -B[i, j];
                }
            }
            return B;
        }
        public static MaTran operator - (MaTran A,MaTran B)
        {
            var C = new MaTran(A.M, A.N);
            C = A + -B;
            return C;
        }
        #endregion
        #region Private Method
        protected void newMaTran(int m, int n)
        {
            MT = new List<List<double>>();
            for (int i = 0; i < m; i++)
            {
                var hang = new List<double>();
                for (int j = 0; j < n; j++)
                {
                    hang.Add(0);
                }
                MT.Add(hang);
            }
        }
        int demSoDongKhacKhong(bool boo)
        {
            int dem = 0;
            for (int i = 0; i < M; i++)
            {
                int tag = 0;
                foreach (var item in MT[i])
                {
                    if (item != 0)
                    {
                        dem++;
                        tag = 1;
                        break;
                    }
                }
                if (tag == 1 || boo)
                {
                    continue;
                }
                foreach (var itemGhep in MTGhep[i])
                {
                    if (itemGhep != 0)
                    {
                        dem++;
                        break;
                    }
                }
            }
            return dem;
        }
        #endregion
    }
}
