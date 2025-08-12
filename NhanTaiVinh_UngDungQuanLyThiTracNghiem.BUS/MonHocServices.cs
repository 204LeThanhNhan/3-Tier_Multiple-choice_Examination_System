using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class MonHocServices
    {
        public List<MON_HOC> LayDanhSachMonHoc()
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            List<MON_HOC> listSubjects = db.MON_HOC.ToList();
            return listSubjects;
        }

        public bool checkKyTuDacBiet(string input)
        {
            // Biểu thức chính quy kiểm tra các ký tự đặc biệt
            string pattern = @"[!@#$%^&*+=\[\]{}\\|;:'""<>,.?]";

            // Kiểm tra chuỗi có chứa ký tự đặc biệt hay không
            return Regex.IsMatch(input, pattern);
        }
        public bool checkTonTaiMonHoc(string maMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            bool exists = db.MON_HOC.Any(gv => gv.MaMon == maMon);

            return exists;//ton tai = true
        }

        public bool checkTonTaiTenMonHoc(string ten)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            bool exists = db.MON_HOC.Any(gv => gv.TenMon.ToLower() == ten.ToLower());

            return exists;//ton tai = true
        }
        public void ThemMonHoc(MON_HOC mh)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            db.MON_HOC.Add(mh);
            db.SaveChanges();
        }
        public void Delete(MON_HOC mh)
        {
            if (mh.MaMon is null || mh.MaMon == "")
            {
                return;
            }
            else
            {
                ThiTracNghiemDB db = new ThiTracNghiemDB();
                MON_HOC dbDelete = db.MON_HOC.FirstOrDefault(p => p.MaMon == mh.MaMon);
                if (dbDelete != null)
                {
                    db.MON_HOC.Remove(dbDelete);
                    db.SaveChanges(); //lưu thay đổi 
                }
            }
        }
        public void CapNhatMH(MON_HOC mh)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            MON_HOC dbUpdate = db.MON_HOC.FirstOrDefault(x => x.MaMon == mh.MaMon);
            dbUpdate.TenMon = mh.TenMon;
            db.SaveChanges();
        }

        public List<MON_HOC> TimKiemMonHoc(string mondangtim)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<MON_HOC> listMon = db.MON_HOC
                                  .Where(s => s.MaMon.Contains(mondangtim) || s.TenMon.Contains(mondangtim))
                                  .ToList();
            return listMon;
        }

        public string TimMaMonHocTheoMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DE_THI dt = db.DE_THI.FirstOrDefault(d => d.MaCaThi == MaCaThi);
            string MaMon = db.CT_DE_THI.Where(c => c.MaDeThi == dt.MaDeThi).Select(c => c.MaMon).FirstOrDefault();
            string mh = db.MON_HOC.Where(m => m.MaMon == MaMon).Select(m => m.MaMon).FirstOrDefault();
            return mh;
        }

        public string TimTenMonHocTheoMaMon(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string tenmon = db.MON_HOC.Where(m => m.MaMon == MaMon).Select(m => m.TenMon).FirstOrDefault();
            return tenmon;
        }

        public void XoaTatCaThongTinMonHoc(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CA_THI> listCaThiLienQuanDenMonHoc = db.CA_THI.Where(ca => ca.MaMon == MaMon).ToList();
            CaThiServices caThiServices = new CaThiServices();
            foreach(CA_THI cathi in listCaThiLienQuanDenMonHoc)
            {
                List<DE_THI> listDeThi = db.DE_THI.Where(de => de.MaCaThi == cathi.MaCaThi).ToList();
                DiemThiServices diemThiServices = new DiemThiServices();
                foreach(DE_THI dethi in listDeThi)
                {
                    CTDeThiServices cTDeThiServices = new CTDeThiServices();
                    DeThiServices deThiServices = new DeThiServices();
                    cTDeThiServices.XoaCTTheoMaDeThi(dethi.MaDeThi);
                    diemThiServices.XoaDiemTheoMaDeThi(dethi.MaDeThi);
                    deThiServices.XoaDeThiTheoMa(dethi.MaDeThi);
                }
                caThiServices.XoaCaThi(cathi.MaCaThi);
            }
        }
    }
}
