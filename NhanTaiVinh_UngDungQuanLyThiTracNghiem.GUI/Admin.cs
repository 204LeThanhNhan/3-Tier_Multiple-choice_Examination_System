using System;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class Admin : Form
    {
        private string MaAdminMoiDangNhap;
        public Admin(string ma)
        {
            InitializeComponent();
            MaAdminMoiDangNhap = ma;
        }
        private readonly AdminServices adminServices = new AdminServices();
        private void Admin_Load(object sender, EventArgs e)
        {
            lbWelcome.Text = adminServices.LayTenTuMaAdminMoiDangNhap(MaAdminMoiDangNhap);
        }

        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_SinhVien frm = new Admin_SinhVien(MaAdminMoiDangNhap);
            frm.Show();
        }

        private void btnGiangVien_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_GiangVien frm = new Admin_GiangVien(MaAdminMoiDangNhap);
            frm.Show();
        }

        private void brnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Cảnh báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.Close();
                Form1 frmDangNhap = new Form1();
                frmDangNhap.Show();
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void btnMonHoc_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_MonHoc frm = new Admin_MonHoc(MaAdminMoiDangNhap);
            frm.Show();
        }

        private void btnCaThi_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_CaThi frm = new Admin_CaThi(MaAdminMoiDangNhap);
            frm.Show();
        }

        private void btnXemDiem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_Diem frm = new Admin_Diem(MaAdminMoiDangNhap);
            frm.Show();
        }
    }
}
