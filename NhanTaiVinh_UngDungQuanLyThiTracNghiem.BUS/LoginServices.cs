using System.Data.SqlClient;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class LoginServices
    {
        public string AdminLogin(string TDN, string MK)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string AdminMoiDangNhap = null;

            string QueryLayMaAdmin = "select MaAdmin from QUAN_LY_NHAN_SU where UsernameADM = @user and PasswordADM = @mk";

            SqlCommand cmd = new SqlCommand(QueryLayMaAdmin, db.Connection);
            cmd.Parameters.AddWithValue("@user", TDN);
            cmd.Parameters.AddWithValue("@mk", MK);
            AdminMoiDangNhap = cmd.ExecuteScalar()?.ToString(); // Trả về mã Admin
            return AdminMoiDangNhap;
        }

        public GIANG_VIEN GiangVienLogin(string TDN, string MK)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            GIANG_VIEN GVMoiDangNhap = null;

            string QueryLayGiangVien = "select * from GIANG_VIEN where UsernameGV = @user and PasswordGV = @mk";

            SqlCommand cmd = new SqlCommand(QueryLayGiangVien, db.Connection);
            cmd.Parameters.AddWithValue("@user", TDN);
            cmd.Parameters.AddWithValue("@mk", MK);

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.Read())
            {
                GVMoiDangNhap = new GIANG_VIEN()
                {
                    MaGiangVien = sqlDataReader["MaGiangVien"].ToString(),
                    TenGiangVien = sqlDataReader["TenGiangVien"].ToString(),
                    SDT = sqlDataReader["SDT"].ToString(),
                    DiaChi = sqlDataReader["DiaChi"].ToString(),
                    Email = sqlDataReader["Email"].ToString()
                };
            }
            sqlDataReader.Close();
            return GVMoiDangNhap;
        }

        public SINH_VIEN SinhVienLogin(string TDN, string MK)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            SINH_VIEN SVMoiDangNhap = null;

            string QueryLaySinhVien = "select * from SINH_VIEN where UsernameSV = @user and PasswordSV = @mk";
            SqlCommand cmd = new SqlCommand(QueryLaySinhVien, db.Connection);
            cmd.Parameters.AddWithValue("@user", TDN);
            cmd.Parameters.AddWithValue("@mk", MK);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.Read())
            {
                SVMoiDangNhap = new SINH_VIEN()
                {
                    MaSinhVien = sqlDataReader["MaSinhVien"].ToString(),
                    HoTen = sqlDataReader["HoTen"].ToString(),
                    NgaySinh = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("NgaySinh")).Date,
                    QueQuan = sqlDataReader["QueQuan"].ToString(),
                    Lop = sqlDataReader["Lop"].ToString(),
                    HinhDaiDien = (byte[])sqlDataReader["HinhDaiDien"]
                };
            }
            return SVMoiDangNhap;
        }
    }
}
