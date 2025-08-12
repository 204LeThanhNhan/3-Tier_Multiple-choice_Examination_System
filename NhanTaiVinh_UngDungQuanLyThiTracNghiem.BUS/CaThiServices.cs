using System;
using System.Collections.Generic;
using System.Linq;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class CaThiServices
    {
        public List<CA_THI> LayDanhSachCaThi()
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CA_THI> listCaThi = db.CA_THI.ToList();
            return listCaThi;
        }


        /// <summary>
        /// Truyền mã ca thi để kiểm tra có mã nào đã tồn tại trước đó chưa
        /// </summary>
        /// <param name="MaCaThi"></param>
        /// <returns>TRUE: đã tồn tại mã, FALSE: chưa tồn tại</returns>
        public bool checkTonTaiMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            if (db.CA_THI.Any(ct => ct.MaCaThi == MaCaThi))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// truyền mã ca thi vào, nếu đã có sinh viên làm bài trong ca thi này thì không thể xóa
        /// ngược lại có thể xóa
        /// </summary>
        /// <param name="MaCaThi"></param>
        /// <returns>true nếu có thể xóa, false không thể xóa</returns>
        public bool KiemTraTruocKhiThaoTacCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<string> listMaDe = db.DE_THI.Where(d => d.MaCaThi == MaCaThi).Select(d => d.MaDeThi).ToList();
            foreach (string s in listMaDe)
            {
                if (db.DIEMs.Any(d => d.MaDeThi == s))
                {
                    return false; // Có phần tử tồn tại, không thể xóa
                }
            }
            return true;
        }

        public void XoaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<string> listMaDe = db.DE_THI.Where(d => d.MaCaThi == MaCaThi).Select(d => d.MaDeThi).ToList();
            foreach (string s in listMaDe)
            {
                CTDeThiServices cTDeThiServices = new CTDeThiServices();
                cTDeThiServices.XoaCTTheoMaDeThi(s);
                DeThiServices deThiServices = new DeThiServices();
                deThiServices.XoaDeThiTheoMa(s);
            }
            CA_THI CaThiCanXoa = db.CA_THI.FirstOrDefault(d => d.MaCaThi == MaCaThi);
            db.CA_THI.Remove(CaThiCanXoa);
            db.SaveChanges();
        }

        /// <summary>
        /// Truyền vào ngày. Kiểm tra xem ngày truyền vào có sau ngày hệ thống hay không
        /// </summary>
        /// <param name="d">Ngày truyền vào</param>
        /// <returns>TRUE: Ngày truyền vào sau ngày hệ thống. Ngược lại FALSE</returns>
        public bool checkNgayCaThiSauNgayHeThong(DateTime d)
        {
            return d >= DateTime.Now.Date;
        }

        /// <summary>
        /// Truyền vào ngày. Kiểm tra xem ngày truyền vào có bằng ngày hệ thống hay không
        /// </summary>
        /// <param name="d">Ngày truyền vào</param>
        /// <returns>TRUE: Ngày truyền vào bằng ngày hệ thống. Ngược lại FALSE</returns>
        public bool checkNgayCaThiBangNgayHeThong(DateTime d)
        {
            return d == DateTime.Now.Date;
        }

        /// <summary>
        /// Truyền vào giờ. Kiểm tra giờ truyền vào có sau giờ hệ thống hay không
        /// </summary>
        /// <param name="Gio"></param>
        /// <returns>TRUE: nếu giờ truyền vào sau giờ hệ thống. Ngược lại FALSE</returns>
        public bool checkGioCaThi(string Gio)
        {
            DateTime gioChonDateTime;

            // Kiểm tra xem chuỗi có thể chuyển thành DateTime hợp lệ không
            if (DateTime.TryParseExact(Gio, "HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out gioChonDateTime))
            {
                // Kiểm tra nếu giờ chọn lớn hơn giờ hệ thống
                return gioChonDateTime.TimeOfDay > DateTime.Now.TimeOfDay;
            }
            return false;
        }

        public void ThemCaThi(CA_THI c)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            db.CA_THI.Add(c);
            db.SaveChanges();
        }

        public string TimTenMonHocTheoMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string MaMon = db.CA_THI.Where(c => c.MaCaThi == MaCaThi).Select(c => c.MaMon).FirstOrDefault();
            MonHocServices monHocServices = new MonHocServices();
            return monHocServices.TimTenMonHocTheoMaMon(MaMon);
        }

        public void CapNhatCaThi(string MaCaThi,DateTime ngaymoi, TimeSpan giobd, TimeSpan gkt, string monmoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            CA_THI CaThiCanCapNhat = db.CA_THI.FirstOrDefault(c => c.MaCaThi == MaCaThi);
            CaThiCanCapNhat.NgayCaThi = ngaymoi;
            CaThiCanCapNhat.GioBatDau = giobd;
            CaThiCanCapNhat.GioKetThuc = gkt;
            CaThiCanCapNhat.MaMon = monmoi;
            db.SaveChanges();
        }

        public CA_THI LayThongTinCaThiTuMaCaThi(string MaCaThi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            CA_THI c = db.CA_THI.FirstOrDefault(ca => ca.MaCaThi == MaCaThi);
            return c;
        }
    }
}
