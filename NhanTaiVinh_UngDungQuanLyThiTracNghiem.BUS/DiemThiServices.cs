using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS
{
    public class DiemThiServices
    {

        public void LuuDiem(string MaDe, string MaSV, int SoCauDung, float Diem, DateTime GioVao, DateTime GioThoat)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DIEM diem = new DIEM();
            diem.MaDeThi = MaDe;
            diem.MaSinhVien = MaSV;
            diem.SoCauDung = SoCauDung;
            diem.DiemThi = Diem;
            diem.ThoiGianVaoLam = GioVao;
            diem.ThoiGianNop = GioThoat;
            db.DIEMs.Add(diem);
            db.SaveChanges();
        }

        public List<DIEM> LayDanhSachDiemThi()
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<DIEM> listDiem = new List<DIEM>();
            listDiem = db.DIEMs.Include("SINH_VIEN").ToList();
            return listDiem;
        }


        public List<DIEM> TimTheoSinhVien(string ma, string ten)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            // Khởi tạo truy vấn với Include SINH_VIEN
            IQueryable<DIEM> query = db.DIEMs.Include("SINH_VIEN");

            if (!string.IsNullOrEmpty(ma))
            {
                query = query.Where(l => l.MaSinhVien == ma);
            }

            if (!string.IsNullOrEmpty(ten))
            {
                query = query.Where(l => l.SINH_VIEN.HoTen.Contains(ten));
            }

            return query.ToList();
        }

        public List<DIEM> LocDiemLiet()
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            
            IQueryable<DIEM> query = db.DIEMs.Include("SINH_VIEN");

            query = query.Where(q => q.DiemThi <= 1);

            return query.ToList();
        }

        public List<DIEM> LocTheoDiem(string DiemTu, string DiemDen, bool Tang, bool Giam)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();

            IQueryable<DIEM> query = db.DIEMs.Include("SINH_VIEN");

            if(!string.IsNullOrEmpty(DiemTu) && !string.IsNullOrEmpty(DiemDen))
            {
                float diemtu = float.Parse(DiemTu);
                float diemden = float.Parse(DiemDen);
                query = query.Where(q => q.DiemThi >= diemtu && q.DiemThi <= diemden);
            }
            if (Tang == true)
            {
                query = query.OrderBy(q => q.DiemThi);
            }
            else if (Giam)
            {
                query = query.OrderByDescending(q => q.DiemThi);
            }
            return query.ToList();
        }
        public void XoaDiemTheoMaDeThi(string MaDe)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            DIEM diem = db.DIEMs.FirstOrDefault(d => d.MaDeThi == MaDe);
            db.DIEMs.Remove(diem);
            db.SaveChanges();
        }
        public void XoaTatCaDiemCuaSV(string MaSV)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            List<DIEM> ListDiemThiCuaSV = db.DIEMs.Where(d => d.MaSinhVien == MaSV).ToList();
            foreach (DIEM diem in ListDiemThiCuaSV)
            {
                CTDeThiServices cTDeThiServices = new CTDeThiServices();
                cTDeThiServices.XoaCTTheoMaDeThi(diem.MaDeThi);

                XoaDiemTheoMaDeThi(diem.MaDeThi);

                DeThiServices deThiServices = new DeThiServices();
                deThiServices.XoaDeThiTheoMa(diem.MaDeThi);
            }
            db.SaveChanges();
        }
    }
}
