using System;
using System.IO;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;
using OfficeOpenXml;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class Admin_GiangVien : Form
    {
        string MaAdminMoiDangNhap;
        public Admin_GiangVien(string ma)
        {
            InitializeComponent();
            MaAdminMoiDangNhap = ma;
        }
        private readonly AdminServices adminServices = new AdminServices();
        private readonly GiangVienServices giangVienServices = new GiangVienServices();
        private void Admin_GiangVien_Load(object sender, EventArgs e)
        {
            lbWelcome.Text = adminServices.LayTenTuMaAdminMoiDangNhap(MaAdminMoiDangNhap);
            HienThiDanhSachGiangVien();
        }
        private void ResetForm()
        {
            txtMaGV.Text = "";
            txtTenGV.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtUsernameGV.Text = "";
            txtPasswordGV.Text = "";
        }
        private void HienThiDanhSachGiangVien()
        {
            var listTeachers = giangVienServices.LayDanhSachGiangVien();
            dgvGiangVien.Rows.Clear();

            foreach (var item in listTeachers)
            {
                int index = dgvGiangVien.Rows.Add();
                dgvGiangVien.Rows[index].Height = 60; // Điều chỉnh chiều cao dòng
                dgvGiangVien.Rows[index].Cells[0].Value = item.MaGiangVien;
                dgvGiangVien.Rows[index].Cells[1].Value = item.TenGiangVien;
                dgvGiangVien.Rows[index].Cells[2].Value = item.SDT;
                dgvGiangVien.Rows[index].Cells[3].Value = item.DiaChi;
                dgvGiangVien.Rows[index].Cells[4].Value = item.Email;
                dgvGiangVien.Rows[index].Cells[5].Value = item.UsernameGV;
                dgvGiangVien.Rows[index].Cells[6].Value = item.UsernameGV;
            }
        }

        private void dgvGiangVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaGV.Text = dgvGiangVien.SelectedRows[0].Cells[0].Value.ToString();
            txtTenGV.Text = dgvGiangVien.SelectedRows[0].Cells[1].Value.ToString();
            txtSDT.Text = dgvGiangVien.SelectedRows[0].Cells[2].Value.ToString();
            txtDiaChi.Text = dgvGiangVien.SelectedRows[0].Cells[3].Value.ToString();
            txtEmail.Text = dgvGiangVien.SelectedRows[0].Cells[4].Value.ToString();
            txtUsernameGV.Text = dgvGiangVien.SelectedRows[0].Cells[5].Value.ToString();
            txtPasswordGV.Text = dgvGiangVien.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void btnResetGV_Click(object sender, EventArgs e)
        {
            ResetForm();
            HienThiDanhSachGiangVien();
        }

        private void btnThemGV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaGV.Text) ||
                string.IsNullOrWhiteSpace(txtTenGV.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUsernameGV.Text) ||
                string.IsNullOrWhiteSpace(txtPasswordGV.Text))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin!!!");
                }

                if (giangVienServices.checkKyTuDacBiet(txtMaGV.Text) == true)
                {
                    throw new Exception("Mã giảng viên không được chứa ký tự đặc biệt!");
                }
                if (giangVienServices.checkKyTuDacBiet(txtTenGV.Text) == true ||
                    giangVienServices.checkSo(txtTenGV.Text) == true)
                {
                    throw new Exception("Họ tên giảng viên chỉ bao gồm chữ cái!");
                }
                if (giangVienServices.checkSoDienThoai(txtSDT.Text) == false)
                {
                    throw new Exception("Số điện thoại chưa hợp lệ!");
                }
                if (giangVienServices.checkDiaChi(txtDiaChi.Text) == true)
                {
                    throw new Exception("Địa chỉ chưa hợp lệ!");
                }
                if (giangVienServices.checkTienToEmail(txtEmail.Text) == true || giangVienServices.checkHauToEmail(txtEmail.Text) == false)
                {
                    throw new Exception("Email chưa hợp lệ!");
                }
                if (giangVienServices.checkTonTaiGiangVien(txtMaGV.Text) == true)
                {
                    throw new Exception("Mã giảng viên này đã tồn tại! Không thể thêm");
                }
                //Tạo đối tượng giang viên, lấy các thông tin từ giao diện gán vào
                GIANG_VIEN gv = new GIANG_VIEN()
                {
                    MaGiangVien = txtMaGV.Text,
                    TenGiangVien = txtTenGV.Text,
                    SDT = txtSDT.Text,
                    DiaChi = txtDiaChi.Text,
                    Email = txtEmail.Text,
                    UsernameGV = txtUsernameGV.Text,
                    PasswordGV = txtPasswordGV.Text
                };
                giangVienServices.ThemGV(gv);
                MessageBox.Show("Thông tin giảng viên đã được lưu thành công!");
                HienThiDanhSachGiangVien();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaGV_Click(object sender, EventArgs e)
        {
            //Tạo đối tượng giảng viên, lấy thông tin từ giao diện về
            GIANG_VIEN gv = new GIANG_VIEN()
            {
                MaGiangVien = txtMaGV.Text,
                TenGiangVien = txtTenGV.Text,
                SDT = txtSDT.Text,
                DiaChi = txtDiaChi.Text,
                Email = txtEmail.Text,
                UsernameGV = txtUsernameGV.Text,
                PasswordGV = txtPasswordGV.Text
            };
            //gọi hàm delete
            giangVienServices.Delete(gv);
            MessageBox.Show("Xoá Giảng Viên thành công", "Thành Công!!!");
            //gọi hàm lấy lại danh sách
            HienThiDanhSachGiangVien();
            ResetForm();
        }

        private void btnCapNhatGV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaGV.Text) ||
                string.IsNullOrWhiteSpace(txtTenGV.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUsernameGV.Text) ||
                string.IsNullOrWhiteSpace(txtPasswordGV.Text))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin!!!");
                }

                if (giangVienServices.checkKyTuDacBiet(txtMaGV.Text) == true)
                {
                    throw new Exception("Mã giảng viên không được chứa ký tự đặc biệt!");
                }
                if (giangVienServices.checkKyTuDacBiet(txtTenGV.Text) == true ||
                    giangVienServices.checkSo(txtTenGV.Text) == true)
                {
                    throw new Exception("Họ tên giảng viên chỉ bao gồm chữ cái!");
                }
                if (giangVienServices.checkSoDienThoai(txtSDT.Text) == false)
                {
                    throw new Exception("Số điện thoại chưa hợp lệ!");
                }
                if (giangVienServices.checkDiaChi(txtDiaChi.Text) == true)
                {
                    throw new Exception("Địa chỉ chưa hợp lệ!");
                }
                if (giangVienServices.checkTienToEmail(txtEmail.Text) == true || giangVienServices.checkHauToEmail(txtEmail.Text) == false)
                {
                    throw new Exception("Email chưa hợp lệ!");
                }
                if (giangVienServices.checkTonTaiGiangVien(txtMaGV.Text) == false)
                {
                    throw new Exception("Mã giảng viên không tồn tại! Không thể cập nhật");
                }
                //Tạo đối tượng giang viên, lấy các thông tin từ giao diện gán vào
                GIANG_VIEN gv = new GIANG_VIEN()
                {
                    MaGiangVien = txtMaGV.Text,
                    TenGiangVien = txtTenGV.Text,
                    SDT = txtSDT.Text,
                    DiaChi = txtDiaChi.Text,
                    Email = txtEmail.Text,
                    UsernameGV = txtUsernameGV.Text,
                    PasswordGV = txtPasswordGV.Text
                };
                giangVienServices.CapNhatGV(gv);
                MessageBox.Show("Thông tin giảng viên đã được cập nhật thành công!");
                HienThiDanhSachGiangVien();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin frm = new Admin(MaAdminMoiDangNhap);
            frm.Show();
        }

        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_SinhVien frm = new Admin_SinhVien(MaAdminMoiDangNhap);
            frm.Show();
        }

        private void btnMonHoc_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_MonHoc frm = new Admin_MonHoc(MaAdminMoiDangNhap);
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

        private void btnXemDiem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_Diem frm = new Admin_Diem(MaAdminMoiDangNhap);
            frm.Show();
        }

        private void ExportExcel(string path)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Giảng Viên");

                    for (int col = 0; col < dgvGiangVien.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = dgvGiangVien.Columns[col].HeaderText;
                        worksheet.Cells[1, col + 1].Style.Font.Bold = true; // Bold for headers
                    }

                    for (int row = 0; row < dgvGiangVien.Rows.Count; row++)
                    {
                        for (int col = 0; col < dgvGiangVien.Columns.Count; col++)
                        {
                            var cellValue = dgvGiangVien.Rows[row].Cells[col].Value?.ToString();
                            worksheet.Cells[row + 2, col + 1].Value = cellValue;
                        }
                    }

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Save to file
                    package.SaveAs(new FileInfo(path));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Đã xảy ra lỗi khi xuất file Excel: {ex.Message}", ex);
            }
        }
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
                Title = "Save Excel File",
                FileName = "GiangVien.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportExcel(saveFileDialog.FileName);
                    MessageBox.Show("Xuất file thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Xuất file lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ImportExcel(string path)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rows = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rows; row++)
                    {
                        GIANG_VIEN gv = new GIANG_VIEN();
                        gv.MaGiangVien = worksheet.Cells[row, 1].Text;
                        gv.TenGiangVien = worksheet.Cells[row, 2].Text;
                        gv.SDT = worksheet.Cells[row, 3].Text;
                        gv.DiaChi = worksheet.Cells[row, 4].Text;
                        gv.Email = worksheet.Cells[row, 5].Text;
                        gv.UsernameGV = worksheet.Cells[row, 6].Text;
                        gv.PasswordGV = worksheet.Cells[row, 7].Text;
                        giangVienServices.ThemGV(gv);
                    }
                }
                MessageBox.Show("Nhập liệu thành công", "Thành Công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachGiangVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog.Title = "Import Excel File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImportExcel(openFileDialog.FileName);
                    //MessageBox.Show("Load file thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("load file không thành công!\n" + ex.Message);
                }
            }
        }
    }
}
