using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class CauHoiServices
    {
        public List<MON_HOC> LayMonThi()
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            return db.MON_HOC.ToList();
        }
        /// <summary>
        /// Nhập vao mã môn học, trả về số lượng câu hỏi hiện có của môn đó
        /// </summary>
        /// <param name="MaMonHoc"></param>
        /// <returns>1 số nguyên đại diện cho số lượng câu hỏi</returns>
        public int LaySTTCauHoi(string MaMonHoc)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string QueryLaySoLuong = "select count(*) from CAU_HOI where MaMon = @ma";
            SqlCommand command = new SqlCommand(QueryLaySoLuong, db.Connection);
            command.Parameters.AddWithValue("@ma", MaMonHoc);
            var result = command.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="macauhoi"></param>
        /// <param name="noidung"></param>
        /// <param name="dokho"></param>
        /// <param name="mamon"></param>
        /// <param name="maGV"></param>
        /// <returns></returns>
        public CAU_HOI TaoCauHoi(string macauhoi, string noidung, string dokho, string mamon, string maGV)
        {
            CAU_HOI ch = new CAU_HOI()
            {
                MaCauHoi = macauhoi,
                NoiDung = noidung,
                DoKho = dokho,
                MaMon = mamon,
                MaGiangVien = maGV
            };
            return ch;
        }
        public DAP_AN TaoDapAn(string madapan, string macauhoi, string maGV, string mamon, string noidungdapan, bool dungsai)
        {
            DAP_AN da = new DAP_AN()
            {
                MaDapAN = madapan,
                MaCauHoi = macauhoi,
                MaGiangVien = maGV,
                MaMon = mamon,
                NoiDungDapAn = noidungdapan,
                DungSai = dungsai // DungSai la kieu bit
            };
            return da;
        }

        public void ThemCauHoi(CAU_HOI h)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            db.CAU_HOI.Add(h);
            db.SaveChanges();
        }

        public void ThemDapAn(DAP_AN d)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            db.DAP_AN.Add(d);
            db.SaveChanges();
        }

        public List<CAU_HOI> LayThongTinCauHoiTheoMaMonHocVaMaGV(string MaMon, string MaGV)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CAU_HOI> listCauHoi = new List<CAU_HOI>();
            listCauHoi = db.CAU_HOI.Where(p => p.MaMon == MaMon && p.MaGiangVien == MaGV).ToList();
            return listCauHoi;
        }

        public CAU_HOI LayThongTinCauHoiTheoMa(string MaCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            CAU_HOI c = new CAU_HOI();
            c = db.CAU_HOI.FirstOrDefault(ch => ch.MaCauHoi == MaCauHoi);
            return c;
        }

        private void XoaCauHoi(string MaCauHoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            CAU_HOI CauHoiCanXoa = new CAU_HOI();
            CauHoiCanXoa = db.CAU_HOI.FirstOrDefault(p => p.MaCauHoi == MaCauHoi);
            db.CAU_HOI.Remove(CauHoiCanXoa);
            db.SaveChanges();
        }
        public void XoaCauHoiTheoMaCauHoi(List<string> listMaCauHoi)
        {
            foreach (string s in listMaCauHoi)
            {
                XoaCauHoi(s);
            }
        }

        public void SuaCauHoiTheoMa(string MaCauHoi, string NoiDungMoi, string DoKhoMoi)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            CAU_HOI CauHoiUpdated = db.CAU_HOI.FirstOrDefault(c => c.MaCauHoi == MaCauHoi);
            CauHoiUpdated.NoiDung = NoiDungMoi;
            CauHoiUpdated.DoKho = DoKhoMoi;
            db.SaveChanges();
        }

        public List<CAU_HOI> TimCauHoi(string maGV, string TT, string MonDangChon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CAU_HOI> list = db.CAU_HOI.Where(c => (c.MaCauHoi.Contains(TT) || c.NoiDung.Contains(TT))
                                                    && c.MaGiangVien == maGV
                                                    && c.MaMon == MonDangChon).ToList();
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        public int TongSoCauHoiCuaMon(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string query = "select count (*) from CAU_HOI where MaMon = @ma";
            SqlCommand sqlCommand = new SqlCommand(query, db.Connection);
            sqlCommand.Parameters.AddWithValue("@ma", MaMon);
            int SoLuong = (int)sqlCommand.ExecuteScalar();
            return SoLuong;
        }

        public int TongSoCauHoiDeCuaMon(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string query = "select count (*) from CAU_HOI where MaMon = @ma AND DoKho = N'Dễ'";
            SqlCommand sqlCommand = new SqlCommand(query, db.Connection);
            sqlCommand.Parameters.AddWithValue("@ma", MaMon);
            int SoLuongCauDe = (int)sqlCommand.ExecuteScalar();
            return SoLuongCauDe;
        }

        public int TongSoCauHoiTrungBinhCuaMon(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string query = "select count (*) from CAU_HOI where MaMon = @ma AND DoKho = N'Trung Bình'";
            SqlCommand sqlCommand = new SqlCommand(query, db.Connection);
            sqlCommand.Parameters.AddWithValue("@ma", MaMon);
            int SoLuongCauTB = (int)sqlCommand.ExecuteScalar();
            return SoLuongCauTB;
        }

        public int TongSoCauHoiKhoCuaMon(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            string query = "select count (*) from CAU_HOI where MaMon = @ma AND DoKho = N'Khó'";
            SqlCommand sqlCommand = new SqlCommand(query, db.Connection);
            sqlCommand.Parameters.AddWithValue("@ma", MaMon);
            int SoLuongCauKho = (int)sqlCommand.ExecuteScalar();
            return SoLuongCauKho;
        }

        public List<CAU_HOI> LayTatCaCauHoiTheoMaMon(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CAU_HOI> listCauHoi = db.CAU_HOI.Where(ch => ch.MaMon == MaMon).ToList();
            return listCauHoi;
        }
        public void XoaTatCaCauHoiTheoMaMon(string MaMon)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<CAU_HOI> listCauHoiCanXoa = db.CAU_HOI.Where(ch => ch.MaMon == MaMon).ToList();
            db.CAU_HOI.RemoveRange(listCauHoiCanXoa);
            db.SaveChanges();
        }

    }
}
