using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class DeThiServices
    {
        //public DataTable SqlHelper { get; private set; }

        public int LayThoiGianLamBaiTheoMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            var thoiGianLamBai = db.DE_THI
            .Where(de => de.MaCaThi == MaCaThi)
            .Select(de => de.ThoiGianLamBai)
            .FirstOrDefault();

            return thoiGianLamBai;
        }

        public DE_THI LayDeThiTheoMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DE_THI d = db.DE_THI.FirstOrDefault(x => x.MaCaThi == MaCaThi);
            return d;
        }

        public void XoaDeThiTheoMa(string MaDe)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DE_THI DeThiCanXoa = new DE_THI();
            DeThiCanXoa = db.DE_THI.FirstOrDefault(d => d.MaDeThi == MaDe);
            db.DE_THI.Remove(DeThiCanXoa);
            db.SaveChanges();
        }

        private int DemSoDeThiChuaDuocDungTheoMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string query = "select count (*) from DE_THI where MaCaThi = @macathi AND SuDung = 0";
            SqlCommand sqlCommand = new SqlCommand(query, db.Connection);
            sqlCommand.Parameters.AddWithValue("@macathi", MaCaThi);
            var number = sqlCommand.ExecuteScalar();
            return Convert.ToInt32(number); //neu tra ve gia tri null thi int32 se giup chuyen thanh so 0
        }

        public void LuuDeThi(DE_THI dt)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            db.DE_THI.Add(dt);
            db.SaveChanges();
        }
        public void TaoDeThi(string MaCaThi, int tglb, int slch, int sld, int sltb, int slk, bool sudung)
        {
            DE_THI d = new DE_THI();
            d.MaDeThi = MaCaThi + (DemSoDeThiChuaDuocDungTheoMaCaThi(MaCaThi) + 1).ToString(); //ma so bat dau tu 1
            d.MaCaThi = MaCaThi;
            d.ThoiGianLamBai = tglb;
            d.SoLuongCauHoi = slch;
            d.SoLuongDe = sld;
            d.SoLuongVua = sltb;
            d.SoLuongKho = slk;
            d.SuDung = sudung;

            LuuDeThi(d);
        }

        public int DemSoDeThiHienCoTheoCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            return db.DE_THI.Count(d => d.MaCaThi == MaCaThi);  // Đếm số lượng đề thi theo MaCaThi
        }
        public DE_THI TaoDeThiMoi(string MaCaThi, int tglb, int slch, int sld, int sltb, int slk, bool sudung)
        {
            DE_THI d = new DE_THI();
            d.MaDeThi = MaCaThi + (DemSoDeThiHienCoTheoCaThi(MaCaThi) + 1).ToString(); 
            d.MaCaThi = MaCaThi;
            d.ThoiGianLamBai = tglb;
            d.SoLuongCauHoi = slch;
            d.SoLuongDe = sld;
            d.SoLuongVua = sltb;
            d.SoLuongKho = slk;
            d.SuDung = sudung;
            LuuDeThi(d);
            return d;
        }

        public DE_THI LayThongTinDeThiTuMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DE_THI d = db.DE_THI.FirstOrDefault(de => de.MaCaThi == MaCaThi);
            return d;
        }

        public DE_THI TimDeThiChuaDuocSuDungTrongCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DE_THI d = null;
            d = db.DE_THI.FirstOrDefault(dethi => dethi.MaCaThi == MaCaThi && dethi.SuDung == false);
            return d;
        }

        public void CapNhatTrangThaiSuDung(DE_THI de)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            //de.SuDung = true;
            //db.SaveChanges();
            var deThi = db.DE_THI.FirstOrDefault(d => d.MaDeThi == de.MaDeThi);
            if (deThi != null)
            {
                deThi.SuDung = true;
                db.SaveChanges();
            }
        }
        public void CapNhatDeThiTheoMaCaThi(string MaCaThi, int tglambai, int slch, int sld, int slv, int slk)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<DE_THI> listDeThi = db.DE_THI.Where(d => d.MaCaThi == MaCaThi).ToList(); //lay nhung de thi thuoc ma ca thi tuong ung
            //cap nhat thong tin tung de thi
            foreach (DE_THI de in listDeThi)
            {
                de.ThoiGianLamBai = tglambai;
                de.SoLuongCauHoi = slch;
                de.SoLuongDe = sld;
                de.SoLuongVua = slv;
                de.SoLuongKho = slk;
            }
            db.SaveChanges();
        }

        public string TimTenMonHocTheoMaDeThi(DE_THI d)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string CaThiHienTai = d.MaCaThi;
            CaThiServices caThiServices = new CaThiServices();
            return caThiServices.TimTenMonHocTheoMaCaThi(CaThiHienTai);
        }

        public string TimMaMonHocTheoMaDeThi(DE_THI d)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            CA_THI CaThiHienTai = db.CA_THI.FirstOrDefault(ca => ca.MaCaThi == d.MaCaThi);
            return CaThiHienTai.MaMon.ToString();
        }

        public DataTable RandomCauHoiChoDeThi(string maMon, int slDe, int slVua, int slKho)
        {
            DE_THI de = new DE_THI();
            return de.RandomCauHoiChoDeThi(maMon,slDe,slVua,slKho);
        }

        public CAU_HOI_IN_EXAM LayMotCauHoiTrongDeThi(string MaCauHoi)
        {
            DE_THI de = new DE_THI();
            return de.LayMotCauHoiTrongDeThi(MaCauHoi);
        }

    }
}
