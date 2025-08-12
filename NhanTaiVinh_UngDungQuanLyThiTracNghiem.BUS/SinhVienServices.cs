using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class SinhVienServices
    {
        public List<SINH_VIEN> LayDanhSachSinhVien()
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            List<SINH_VIEN> listStudent = db.SINH_VIEN.ToList();
            return listStudent;
        }

        public byte[] LayHinhSVTuDatabase(string MaSV)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            byte[] hinh = db.SINH_VIEN.Where(sv => sv.MaSinhVien == MaSV).Select(s => s.HinhDaiDien).FirstOrDefault();
            return hinh;
        }
        public bool checkKyTuDacBiet(string input)
        {
            // Biểu thức chính quy kiểm tra các ký tự đặc biệt
            string pattern = @"[!@#$%^&*()=+\-*/\[\]{}\\|;:'""<>,.?/]";

            // Kiểm tra chuỗi có chứa ký tự đặc biệt hay không
            return Regex.IsMatch(input, pattern);
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
        public bool checkTonTaiSinhVien(string maSV)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            bool exists = db.SINH_VIEN.Any(sv => sv.MaSinhVien == maSV);

            return exists;//ton tai = true
        }

        public void ThemSV(SINH_VIEN s)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            db.SINH_VIEN.Add(s);
            db.SaveChanges();
        }
        public void CapNhatSV(SINH_VIEN s)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            SINH_VIEN dbUpdate = db.SINH_VIEN.FirstOrDefault(x => x.MaSinhVien == s.MaSinhVien);
            dbUpdate.HoTen = s.HoTen;
            dbUpdate.NgaySinh = s.NgaySinh;
            dbUpdate.Lop = s.Lop;
            dbUpdate.QueQuan = s.QueQuan;
            dbUpdate.UsernameSV = s.UsernameSV;
            dbUpdate.PasswordSV = s.PasswordSV;
            dbUpdate.HinhDaiDien = s.HinhDaiDien;

            db.SaveChanges();


        }
        public SINH_VIEN TimTheoMa(string ma)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            SINH_VIEN student = null;
            student = db.SINH_VIEN.FirstOrDefault(s => s.MaSinhVien == ma);
            return student;
        }

        public void Delete(SINH_VIEN s)
        {
            if (s.MaSinhVien is null || s.MaSinhVien == "")
            {
                return;
            }
            else
            {
                ThiTracNghiemDB db = new ThiTracNghiemDB();
                SINH_VIEN dbDelete = db.SINH_VIEN.FirstOrDefault(p => p.MaSinhVien == s.MaSinhVien);
                if (dbDelete != null)
                {
                    db.SINH_VIEN.Remove(dbDelete);
                    db.SaveChanges(); //lưu thay đổi 
                }
            }
        }

        public async Task<DateTime> LayNgayGioTuAPI()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);  // Thời gian chờ tối đa 20 giây
            // API lấy giờ theo múi giờ Việt Nam
            string apiUrl = "https://www.timeapi.io/api/time/current/zone?timeZone=Asia%2FHo_Chi_Minh";
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                // Đọc dữ liệu JSON từ API
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<WorldTimeAPISevices>(responseBody);

                // Chuyển đổi datetime từ string sang DateTime
                DateTime gioAPI = DateTime.Parse(result.datetime);
                return gioAPI;  // Trả về DateTime
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                return DateTime.Now; // quá thời gian chờ -> trả về giờ hệ thống
            }
            catch (HttpRequestException ex)
            {
                return DateTime.Now; // lỗi kết nối mạng -> trả về giờ hệ thống
            }
        }

        public List<CA_THI> LayTatCaCaThiDangDienRa(DateTime ThoiGianHienTai)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            string query = @"
                            SELECT * FROM CA_THI 
                            WHERE NgayCaThi = @NgayCaThi AND
                            @GioHienTai BETWEEN GioBatDau AND GioKetThuc";

            SqlCommand cmd = new SqlCommand(query, db.Connection);
            cmd.Parameters.AddWithValue("@NgayCaThi", ThoiGianHienTai.Date);
            cmd.Parameters.AddWithValue("@GioHienTai", ThoiGianHienTai.TimeOfDay);

            SqlDataReader reader = cmd.ExecuteReader();
            List<CA_THI> caThiDangDienRa = new List<CA_THI>();

            while (reader.Read())
            {
                CA_THI caThi = new CA_THI
                {
                    MaCaThi = reader["MaCaThi"].ToString(),
                    NgayCaThi = Convert.ToDateTime(reader["NgayCaThi"]),
                    GioBatDau = (TimeSpan)reader["GioBatDau"],
                    GioKetThuc = (TimeSpan)reader["GioKetThuc"],
                    MaMon = reader["MaMon"].ToString(),
                };

                caThiDangDienRa.Add(caThi);
            }

            return caThiDangDienRa;
        }

    }
}
