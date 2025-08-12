using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;
using OfficeOpenXml;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class Admin_MonHoc : Form
    {
        string MaAdminMoiDangNhap;
        public Admin_MonHoc(string ma)
        {
            InitializeComponent();
            MaAdminMoiDangNhap = ma;
        }
        private readonly AdminServices adminServices = new AdminServices();
        private readonly MonHocServices monHocServices = new MonHocServices();
        private readonly CauHoiServices cauHoiServices = new CauHoiServices();
        private readonly DapAnServices dapAnServices = new DapAnServices();
        private void HienThiDanhSachMonHoc()
        {
            var listMonHoc = monHocServices.LayDanhSachMonHoc();
            dgvMonHoc.Rows.Clear();

            foreach (var item in listMonHoc)
            {
                int index = dgvMonHoc.Rows.Add();
                dgvMonHoc.Rows[index].Height = 30;
                dgvMonHoc.Rows[index].Cells[0].Value = item.MaMon;
                dgvMonHoc.Rows[index].Cells[1].Value = item.TenMon;
            }
        }
        private void Admin_MonHoc_Load(object sender, EventArgs e)
        {
            lbWelcome.Text = adminServices.LayTenTuMaAdminMoiDangNhap(MaAdminMoiDangNhap);
            HienThiDanhSachMonHoc();
        }

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaMon.Text = dgvMonHoc.SelectedRows[0].Cells[0].Value.ToString();
            txtTenMon.Text = dgvMonHoc.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            HienThiDanhSachMonHoc();
            ResetForm();
        }

        private void ResetForm()
        {
            txtMaMon.Text = "";
            txtTenMon.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaMon.Text) || string.IsNullOrWhiteSpace(txtTenMon.Text))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin cho môn học");
                }
                if (monHocServices.checkKyTuDacBiet(txtMaMon.Text) == true)
                {
                    throw new Exception("Mã môn học không được chứa ký tự đặc biệt");
                }
                if (monHocServices.checkKyTuDacBiet(txtTenMon.Text) == true)
                {
                    throw new Exception("Tên môn học không được chứa ký tự đặc biệt");
                }
                if (monHocServices.checkTonTaiMonHoc(txtMaMon.Text) == true)
                {
                    throw new Exception("Mã môn học đã tồn tại\nKhông thể thêm");
                }
                if (monHocServices.checkTonTaiTenMonHoc(txtTenMon.Text) == true)
                {
                    throw new Exception("Tên môn học đã tồn tại\n2 môn học không thể trùng tên");
                }
                MON_HOC mh = new MON_HOC()
                {
                    MaMon = txtMaMon.Text,
                    TenMon = txtTenMon.Text,
                };
                monHocServices.ThemMonHoc(mh);
                MessageBox.Show("Thông tin môn học đã được lưu thành công!");
                HienThiDanhSachMonHoc();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn xóa môn học này?\nKhi xóa, tất cả dữ liệu trong CA THI, ĐỀ THI, và ĐIỂM của sinh viên liên quan đến môn học đều bị xóa cùng", "Thông báo!",MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                //Tạo đối tượng giảng viên, lấy thông tin từ giao diện về
                MON_HOC mh = new MON_HOC()
                {
                    MaMon = txtMaMon.Text,
                    TenMon = txtTenMon.Text,
                };
                //gọi hàm delete
                string mamon = txtMaMon.Text;
                List<CAU_HOI> listCauHoiCanXoa = cauHoiServices.LayTatCaCauHoiTheoMaMon(mamon);
                monHocServices.XoaTatCaThongTinMonHoc(mamon);
                dapAnServices.XoaTatCaDapAnTheoMaCauHoi(listCauHoiCanXoa);
                cauHoiServices.XoaTatCaCauHoiTheoMaMon(mamon);
                monHocServices.Delete(mh);
                MessageBox.Show("Xoá môn học thành công", "Thành Công!!!");
                //gọi hàm lấy lại danh sách
                HienThiDanhSachMonHoc();
                ResetForm();
            }
        }

        private void btnCapNhatMon_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaMon.Text) || string.IsNullOrWhiteSpace(txtTenMon.Text))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin cho môn học");
                }
                if (monHocServices.checkKyTuDacBiet(txtMaMon.Text) == true)
                {
                    throw new Exception("Mã môn học không được chứa ký tự đặc biệt");
                }
                if (monHocServices.checkKyTuDacBiet(txtTenMon.Text) == true)
                {
                    throw new Exception("Tên môn học không được chứa ký tự đặc biệt");
                }
                if (monHocServices.checkTonTaiMonHoc(txtMaMon.Text) == false)
                {
                    throw new Exception("Mã môn học không tồn tại! Không thể cập nhật");
                }
                if (monHocServices.checkTonTaiTenMonHoc(txtTenMon.Text) == true)
                {
                    throw new Exception("Tên môn học đã tồn tại\nSau khi cập nhật không thể có 2 môn học trùng tên");
                }
                MON_HOC mh = new MON_HOC()
                {
                    MaMon = txtMaMon.Text,
                    TenMon = txtTenMon.Text,
                };
                monHocServices.CapNhatMH(mh);
                MessageBox.Show("Thông tin môn học đã được cập nhật thành công!");
                HienThiDanhSachMonHoc();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTimMon_Click(object sender, EventArgs e)
        {
            try
            {
                ThiTracNghiemDB db = new ThiTracNghiemDB();
                string mondangtim = txtTimMonHoc.Text.Trim();
                if (string.IsNullOrWhiteSpace(mondangtim))
                {
                    MessageBox.Show("Vui lòng nhập thông tin để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                List<MON_HOC> listMonHoc = monHocServices.TimKiemMonHoc(mondangtim);
                if (listMonHoc.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy Môn Học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachMonHoc();
                }
                else
                {
                    dgvMonHoc.Rows.Clear();

                    foreach (var item in listMonHoc)
                    {
                        int index = dgvMonHoc.Rows.Add();
                        dgvMonHoc.Rows[index].Height = 30;
                        dgvMonHoc.Rows[index].Cells[0].Value = item.MaMon;
                        dgvMonHoc.Rows[index].Cells[1].Value = item.TenMon;
                    }
                }
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

        private void btnMonHoc_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
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
        private void ExportExcel(string path)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Môn Học");

                    for (int col = 0; col < dgvMonHoc.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = dgvMonHoc.Columns[col].HeaderText;
                        worksheet.Cells[1, col + 1].Style.Font.Bold = true; // Bold for headers
                    }

                    for (int row = 0; row < dgvMonHoc.Rows.Count; row++)
                    {
                        for (int col = 0; col < dgvMonHoc.Columns.Count; col++)
                        {
                            var cellValue = dgvMonHoc.Rows[row].Cells[col].Value?.ToString();
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
                FileName = "MonHoc.xlsx"
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
                        MON_HOC mh = new MON_HOC();
                        mh.MaMon = worksheet.Cells[row, 1].Text;
                        mh.TenMon = worksheet.Cells[row, 2].Text;
                        monHocServices.ThemMonHoc(mh);
                    }
                }
                HienThiDanhSachMonHoc();
                MessageBox.Show("Nhập liệu thành công", "Thành Công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
