using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class GiangVienServices
    {
        public List<GIANG_VIEN> LayDanhSachGiangVien()
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            List<GIANG_VIEN> listTeachers = db.GIANG_VIEN.ToList();
            return listTeachers;
        }
        public bool checkKyTuDacBiet(string input)
        {
            // Biểu thức chính quy kiểm tra các ký tự đặc biệt
            string pattern = @"[!@#$%^&*()=+\-*/\[\]{}\\|;:'""<>,.?/]";

            // Kiểm tra chuỗi có chứa ký tự đặc biệt hay không
            return Regex.IsMatch(input, pattern);
        }
        public bool checkHauToEmail(string input)
        {
            return input.EndsWith("@gmail.com");
        }
        public bool checkTienToEmail(string input)
        {
            string TienToMail = input.Substring(0, input.IndexOf("@"));
            return checkKyTuDacBiet(TienToMail);
        }
        public bool checkDiaChi(string input)
        {
            // Biểu thức chính quy kiểm tra ký tự đặc biệt
            string pattern = @"[!@#$%^&*_+=()\[\]'><?:]+";

            // Kiểm tra chuỗi có chứa ký tự đặc biệt hay không
            return Regex.IsMatch(input, pattern);
        }
        public bool checkSoDienThoai(string soDienThoai)
        {
            // Kiểm tra nếu số điện thoại chỉ bao gồm các chữ số và có độ dài từ 10 đến 11 chữ số
            Regex regex = new Regex(@"^\d{10,11}$");
            return regex.IsMatch(soDienThoai);
        }
        public bool checkSo(string input)
        {
            foreach (char c in input)
            {
                if (Char.IsDigit(c))
                {
                    return true;
                }
            }

            return false;
        }
        public bool checkTonTaiGiangVien(string maGV)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            bool exists = db.GIANG_VIEN.Any(gv => gv.MaGiangVien == maGV);

            return exists;//ton tai = true
        }
        public void ThemGV(GIANG_VIEN gv)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            db.GIANG_VIEN.Add(gv);
            db.SaveChanges();
        }

        public void Delete(GIANG_VIEN gv)
        {
            if (gv.MaGiangVien is null || gv.MaGiangVien == "")
            {
                return;
            }
            else
            {
                ThiTracNghiemDB db = new ThiTracNghiemDB();
                GIANG_VIEN dbDelete = db.GIANG_VIEN.FirstOrDefault(p => p.MaGiangVien == gv.MaGiangVien);
                if (dbDelete != null)
                {
                    db.GIANG_VIEN.Remove(dbDelete);
                    db.SaveChanges(); //lưu thay đổi 
                }
            }
        }
        public void CapNhatGV(GIANG_VIEN gv)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            GIANG_VIEN dbUpdate = db.GIANG_VIEN.FirstOrDefault(x => x.MaGiangVien == gv.MaGiangVien);
            dbUpdate.TenGiangVien = gv.TenGiangVien;
            dbUpdate.SDT = gv.SDT;
            dbUpdate.DiaChi = gv.DiaChi;
            dbUpdate.Email = gv.Email;
            dbUpdate.UsernameGV = gv.UsernameGV;
            dbUpdate.PasswordGV = gv.PasswordGV;

            db.SaveChanges();


        }

        public string LayTenGVTheoMaGV(string maGV)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            var giangVien = db.GIANG_VIEN.FirstOrDefault(gv => gv.MaGiangVien == maGV);
            return giangVien.TenGiangVien.ToString();
        }
    }
}
