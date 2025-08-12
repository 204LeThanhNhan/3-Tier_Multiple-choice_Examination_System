using System.Collections.Generic;
using System.Linq;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class CTDeThiServices
    {
        public void XoaCTTheoMaDeThi(string MaDe)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CT_DE_THI> listCTDeThi = db.CT_DE_THI.Where(ct => ct.MaDeThi == MaDe).ToList();
            db.CT_DE_THI.RemoveRange(listCTDeThi);// Xóa từng dòng
            db.SaveChanges();
        }

        public void LuuChiTietDeThi(string MaDeThi, List<CAU_HOI> listCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CT_DE_THI> listCTDeThi = new List<CT_DE_THI>();
            foreach(CAU_HOI ch in listCauHoi)
            {
                CT_DE_THI ct = new CT_DE_THI();
                ct.MaDeThi = MaDeThi;
                ct.MaCauHoi = ch.MaCauHoi;
                ct.MaMon = ch.MaMon;
                ct.MaGiangVien = ch.MaGiangVien;
                listCTDeThi.Add(ct);
            }
            db.CT_DE_THI.AddRange(listCTDeThi);
            db.SaveChanges();
        }
    }
}
