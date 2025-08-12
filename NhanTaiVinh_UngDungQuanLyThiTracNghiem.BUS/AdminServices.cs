using System.Data.SqlClient;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class AdminServices
    {
        public string LayTenTuMaAdminMoiDangNhap(string ma)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string QueryLayTenAdmin = "select TenAdmin from QUAN_LY_NHAN_SU where MaAdmin = @ma";
            SqlCommand cmd = new SqlCommand(QueryLayTenAdmin, db.Connection);
            cmd.Parameters.AddWithValue("@ma", ma);
            return cmd.ExecuteScalar()?.ToString(); // Trả về ten Admin
        }


    }
}
