using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class SinhVien : Form
    {
        private SINH_VIEN SVMoiDangNhap;
        public SinhVien(SINH_VIEN s)
        {
            InitializeComponent();
            SVMoiDangNhap = s;
        }
        //LNCH2024
        //12162024
        SinhVienServices SinhVienServices = new SinhVienServices();
        DeThiServices deThiServices = new DeThiServices();
        MonHocServices monHocServices = new MonHocServices();
        CaThiServices caThiServices = new CaThiServices();
        private Image ConvertByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Trả về kiểu DateTime</returns>
        private async Task<DateTime> LayNgayGio()
        {
            DateTime test = await SinhVienServices.LayNgayGioTuAPI();
            return test;
        }
        private async void SinhVien_Load(object sender, EventArgs e)
        {
            lbMaSV.Text = SVMoiDangNhap.MaSinhVien.ToString();
            lbHoTen.Text = SVMoiDangNhap.HoTen.ToString();
            lbNgaySinh.Text = SVMoiDangNhap.NgaySinh.ToString("dd/MM/yyyy");
            lbLop.Text = SVMoiDangNhap.Lop.ToString();
            lbQuequan.Text = SVMoiDangNhap.QueQuan.ToString();
            pictureBox1.Image = ConvertByteArrayToImage(SVMoiDangNhap.HinhDaiDien);
            // chặn sự kiện SelectedIndexChanged tránh lỗi
            cbCaThi.SelectedIndexChanged -= cbCaThi_SelectedIndexChanged;

            List<CA_THI> CaThiDangDienRa = SinhVienServices.LayTatCaCaThiDangDienRa(await LayNgayGio());
            if (CaThiDangDienRa.Count != 0)
            {
                cbCaThi.DataSource = CaThiDangDienRa;
                cbCaThi.DisplayMember = "MaCaThi";
                cbCaThi.ValueMember = "MaCaThi";
                cbCaThi.Text = "";
                // Kích hoạt lại SelectedIndexChanged
                cbCaThi.SelectedIndexChanged += cbCaThi_SelectedIndexChanged;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Cảnh báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.Close();
                Form1 frmDangNhap = new Form1();
                frmDangNhap.Show();
            }
        }

        private void cbCaThi_SelectedIndexChanged(object sender, EventArgs e)
        {
            string MaCaThiDangChon = cbCaThi.Text.Trim();
            DE_THI d = deThiServices.LayThongTinDeThiTuMaCaThi(MaCaThiDangChon);

            lbThoiGianLamBai.Text = d.ThoiGianLamBai.ToString() + " phút";
            lbMonThi.Text = caThiServices.TimTenMonHocTheoMaCaThi(MaCaThiDangChon);
            lbSoCau.Text = d.SoLuongCauHoi.ToString();
        }
        private async Task<DateTime> LayNgayGioHienTai()
        {
            DateTime test = await SinhVienServices.LayNgayGioTuAPI();
            return test;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string MaCaThiDangChon = cbCaThi.Text.Trim();
                DateTime HienTai = await LayNgayGioHienTai();
                CA_THI CaThiDangChon = caThiServices.LayThongTinCaThiTuMaCaThi(MaCaThiDangChon);
                TimeSpan ThoiGianTre = HienTai.TimeOfDay - CaThiDangChon.GioBatDau;
                if (ThoiGianTre.TotalMinutes > 15)
                {
                    MessageBox.Show("Bạn không thể thực hiện ca thi vì đã vào trễ hơn 15 phút kể từ lúc ca thi bắt đầu",
                                    "THÔNG BÁO VI PHẠM QUY CHẾ THI!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DE_THI d = deThiServices.TimDeThiChuaDuocSuDungTrongCaThi(MaCaThiDangChon);
                if (d == null)
                {
                    DE_THI DeThiMau = deThiServices.LayThongTinDeThiTuMaCaThi(MaCaThiDangChon);
                    int tglb = DeThiMau.ThoiGianLamBai;
                    int slch = DeThiMau.SoLuongCauHoi;
                    int sld = DeThiMau.SoLuongDe;
                    int slv = DeThiMau.SoLuongVua;
                    int slk = DeThiMau.SoLuongKho;
                    bool trangthai = true;
                    d = deThiServices.TaoDeThiMoi(MaCaThiDangChon, tglb, slch, sld, slv, slk, trangthai);
                }
                deThiServices.CapNhatTrangThaiSuDung(d);
                this.Hide();
                ThucHienThi frm = new ThucHienThi(SVMoiDangNhap, d);//truyen SVMoiDangNhap, DeThi vao
                frm.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
