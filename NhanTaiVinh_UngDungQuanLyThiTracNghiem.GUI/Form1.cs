using System;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly LoginServices loginServices = new LoginServices();
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenDangNhap.Text == "" || txtMatKhau.Text == "" || (rdGiangVien.Checked == false && rdSinhVien.Checked == false && rdQuanLy.Checked == false))
                {
                    throw new Exception("Vui lòng nhập đầy đủ Tên Đăng Nhập và Mật Khẩu và Chọn loại đăng nhập");
                }
                string TDN = txtTenDangNhap.Text;
                string MK = txtMatKhau.Text;
                if (rdQuanLy.Checked == true)
                {
                    //xu ly dang nhap cho admin
                    string MaAdminMoiDangNhap = loginServices.AdminLogin(TDN, MK);
                    if (MaAdminMoiDangNhap == null)
                    {
                        throw new Exception("Username hoặc Passowrd không đúng!\nVui lòng kiểm tra lại");
                    }
                    else
                    {
                        this.Hide();
                        Admin frm = new Admin(MaAdminMoiDangNhap);
                        frm.Show();
                    }

                }
                else if (rdGiangVien.Checked == true)
                {
                    string TDNGV = txtTenDangNhap.Text;
                    string MKGV = txtMatKhau.Text;

                    GIANG_VIEN GiangVienMoiDangNhap = loginServices.GiangVienLogin(TDN, MK);
                    if (GiangVienMoiDangNhap == null)
                    {
                        throw new Exception("Username hoặc Passowrd không đúng!\nVui lòng kiểm tra lại");
                    }
                    else
                    {
                        this.Hide();
                        GiangVien frm = new GiangVien(GiangVienMoiDangNhap);
                        frm.Show();
                    }

                }
                else
                {
                    string TDNSV = txtTenDangNhap.Text;
                    string MKSV = txtMatKhau.Text;
                    SINH_VIEN SinhVienMoiDangNhap = loginServices.SinhVienLogin(TDN, MK);
                    if (SinhVienMoiDangNhap == null)
                    {
                        throw new Exception("Username hoặc Passowrd không đúng!\nVui lòng kiểm tra lại");
                    }
                    else
                    {
                        this.Hide();
                        SinhVien frm = new SinhVien(SinhVienMoiDangNhap);
                        frm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
