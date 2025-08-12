using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class KetQuaThi : Form
    {
        private SINH_VIEN SVDangDangNhap;
        DE_THI DeThiVuaChon;
        string TenMon;
        float Diem;
        int SoCauDung;
        public KetQuaThi(SINH_VIEN s, DE_THI d, string tenMon,float diemthi, float soCauDung)
        {
            InitializeComponent();
            SVDangDangNhap = s;
            DeThiVuaChon = d;
            TenMon = tenMon;
            Diem = diemthi ;
            SoCauDung = int.Parse(soCauDung.ToString());
        }
        private Image ConvertByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
        private void KetQuaThi_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = ConvertByteArrayToImage(SVDangDangNhap.HinhDaiDien);
            lbMaSV.Text = SVDangDangNhap.MaSinhVien.ToString();
            lbHoTen.Text = SVDangDangNhap.HoTen.ToString();
            lbNgaySinh.Text = SVDangDangNhap.NgaySinh.ToString("dd/MM/yyyy");
            lbLop.Text = SVDangDangNhap.Lop.ToString();
            lbQuequan.Text = SVDangDangNhap.QueQuan.ToString();
            lbMaDe.Text = DeThiVuaChon.MaDeThi.ToString();
            lbTenMonThi.Text = TenMon;
            lbSoCauHoi.Text = DeThiVuaChon.SoLuongCauHoi.ToString();
            lbSoCauDung.Text = SoCauDung.ToString();
            lbDiem.Text = Diem.ToString();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 frm = new Form1();
            frm.Show();
        }
    }
}
