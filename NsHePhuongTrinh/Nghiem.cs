using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NsMaTran;

namespace NsHePhuongTrinh
{
    public enum Status
    {
        VoNghiem,
        NghiemDuyNhat,
        VoSoNghiem
    }
    public enum LoaiNghiem
    {
        PhuThuoc,
        TuDo
    }
    class Nghiem : MaTran
    {
        public Nghiem(Status tag, int length)
        {
            status = tag;
            if (tag == Status.NghiemDuyNhat)
            {
                newMaTran(length, 1);
            }
            else if (tag == Status.VoSoNghiem)
            {
                newMaTran(length, length + 1);
                for (int i = 0; i < length; i++)
                {
                    Loai.Add(LoaiNghiem.TuDo);
                }
            }
        }
        public Nghiem(string filePath) : base(filePath)
        {

        }
        public Nghiem(MaTran mt) : base(mt)
        {
            if (mt.M != 0)
            {
                if (mt.N > 1)
                {
                    status = Status.VoSoNghiem;
                }
                else
                {
                    status = Status.NghiemDuyNhat;
                }
            }
        }
        #region Member
        public Status status = Status.VoNghiem;
        public List<LoaiNghiem> Loai { get; set; } = new List<LoaiNghiem>();
        #endregion
        #region Public Method
        public new Nghiem Copy()
        {
            var n = new Nghiem(status, M);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    n[i, j] = this[i, j];
                }
                if (status == Status.VoSoNghiem)
                {
                    n.Loai[i] = Loai[i];
                }
            }
            return n;
        }
        public override string ToString()
        {
            string s = "";
            switch (status)
            {
                default:
                    return "";
                case Status.VoNghiem:
                    return "Hệ vô nghiệm.";
                case Status.NghiemDuyNhat:
                    s = "Hệ có nghiệm duy nhất:\n";
                    for (int i = 0; i < MT.Count; i++)
                    {
                        s += string.Format("x[{0}] = {1}\n", i, MT[i][0]);
                    }
                    return s;
                case Status.VoSoNghiem:
                    s = "Hệ có vô số nghiệm:\n";
                    for (int i = 0; i < MT.Count; i++)
                    {
                        if (Loai[i] == LoaiNghiem.TuDo)
                        {
                            s += string.Format("x[{0}] tự do\n", i);
                        }
                        else
                        {
                            s += string.Format("x[{0}] = ", i);
                            var bi = MT[i][M];
                            if (bi != 0)
                            {
                                s += string.Format("{0} ", bi);
                            }
                            for (int j = 0; j < M; j++)
                            {
                                if (MT[i][j] > 0)
                                {
                                    s += string.Format("+ {0}.x[{1}] ", MT[i][j], j);
                                }
                                else if (MT[i][j] < 0)
                                {
                                    s += string.Format("- {0}.x[{1}] ", -MT[i][j], j);
                                }
                            }
                            s += "\n";
                        }
                    }
                    return s;
            }
        }
        #endregion
    }
}
