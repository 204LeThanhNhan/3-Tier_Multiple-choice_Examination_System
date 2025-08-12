using System.Collections.Generic;
using System.Linq;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class DapAnServices
    {

        public string LayNoiDungDapAnATheoMaCauHoi(string MaCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN dapan = new DAP_AN();
            dapan = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("A"));
            return dapan.NoiDungDapAn;
        }

        public string LayNoiDungDapAnBTheoMaCauHoi(string MaCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN dapan = new DAP_AN();
            dapan = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("B"));
            return dapan.NoiDungDapAn;
        }

        public string LayNoiDungDapAnCTheoMaCauHoi(string MaCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN dapan = new DAP_AN();
            dapan = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("C"));
            return dapan.NoiDungDapAn;
        }

        public string LayNoiDungDapAnDTheoMaCauHoi(string MaCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN dapan = new DAP_AN();
            dapan = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("D"));
            return dapan.NoiDungDapAn;
        }

        public string DapAnDungTheoMaCauHoi(string MaCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN DapAnDung = new DAP_AN();
            DapAnDung = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.DungSai == true);
            return DapAnDung.MaDapAN.Substring(DapAnDung.MaDapAN.Length - 1);
        }
        private void XoaDapAn(string MaDapAN)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN DapAnCanXoa = db.DAP_AN.FirstOrDefault(p => p.MaDapAN == MaDapAN);
            db.DAP_AN.Remove(DapAnCanXoa);
            db.SaveChanges();
        }
        public void XoaDapAnTheoMaCauHoi(List<string> listMaCauHoi)
        {
            foreach (string s in listMaCauHoi)
            {
                XoaDapAn(s + "A");
                XoaDapAn(s + "B");
                XoaDapAn(s + "C");
                XoaDapAn(s + "D");
            }
        }

        public void SuaDapAnATheoMaCauHoi(string MaCauHoi, string NoiDungDapAn, bool DungSai)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN DapAnCanSua = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("A"));
            DapAnCanSua.NoiDungDapAn = NoiDungDapAn;
            DapAnCanSua.DungSai = DungSai;
            db.SaveChanges();
        }

        public void SuaDapAnBTheoMaCauHoi(string MaCauHoi, string NoiDungDapAn, bool DungSai)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN DapAnCanSua = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("B"));
            DapAnCanSua.NoiDungDapAn = NoiDungDapAn;
            DapAnCanSua.DungSai = DungSai;
            db.SaveChanges();
        }

        public void SuaDapAnCTheoMaCauHoi(string MaCauHoi, string NoiDungDapAn, bool DungSai)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN DapAnCanSua = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("C"));
            DapAnCanSua.NoiDungDapAn = NoiDungDapAn;
            DapAnCanSua.DungSai = DungSai;
            db.SaveChanges();
        }

        public void SuaDapAnDTheoMaCauHoi(string MaCauHoi, string NoiDungDapAn, bool DungSai)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DAP_AN DapAnCanSua = db.DAP_AN.FirstOrDefault(d => d.MaCauHoi == MaCauHoi && d.MaDapAN.EndsWith("D"));
            DapAnCanSua.NoiDungDapAn = NoiDungDapAn;
            DapAnCanSua.DungSai = DungSai;
            db.SaveChanges();
        }

        public void XoaTatCaDapAnTheoMaCauHoi(List<CAU_HOI> listCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            foreach(CAU_HOI i in listCauHoi)
            {
                List<DAP_AN> listDapAn = db.DAP_AN.Where(d => d.MaCauHoi == i.MaCauHoi).ToList();
                db.DAP_AN.RemoveRange(listDapAn);
            }
            db.SaveChanges();
        }
    }
}
