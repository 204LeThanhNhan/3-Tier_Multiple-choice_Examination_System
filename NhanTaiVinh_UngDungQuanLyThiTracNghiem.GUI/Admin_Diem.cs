using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;
using OfficeOpenXml;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class Admin_Diem : Form
    {
        private string MaAdminMoiDangNhap;
        public Admin_Diem(string ma)
        {
            InitializeComponent();
            MaAdminMoiDangNhap = ma;
        }

        private readonly DiemThiServices diemThiServices = new DiemThiServices();
        private readonly AdminServices adminServices = new AdminServices();
        private readonly CauHoiServices cauHoiServices = new CauHoiServices();
        private void HienThiTatCaDiem()
        {
            List<DIEM> list = diemThiServices.LayDanhSachDiemThi();
            dgvDiem.Rows.Clear();

            foreach (var item in list)
            {
                int index = dgvDiem.Rows.Add();
                dgvDiem.Rows[index].Height = 30;
                dgvDiem.Rows[index].Cells[0].Value = item.MaDeThi;
                dgvDiem.Rows[index].Cells[1].Value = item.MaSinhVien;
                dgvDiem.Rows[index].Cells[2].Value = item.SINH_VIEN.HoTen;
                dgvDiem.Rows[index].Cells[3].Value = item.SINH_VIEN.NgaySinh.ToString("dd/MM/yyyy");
                dgvDiem.Rows[index].Cells[4].Value = item.SINH_VIEN.Lop;
                dgvDiem.Rows[index].Cells[5].Value = item.DiemThi.ToString("0.0");
                dgvDiem.Rows[index].Cells[6].Value = item.ThoiGianVaoLam.ToString("dd/MM/yyyy _ HH:mm:ss");
                dgvDiem.Rows[index].Cells[7].Value = item.ThoiGianNop.ToString("dd/MM/yyyy _ HH:mm:ss");
            }
        }

        private void HienThiDiemTheoSV(List<DIEM> l)
        {
            dgvDiem.Rows.Clear();

            foreach (var item in l)
            {
                int index = dgvDiem.Rows.Add();
                dgvDiem.Rows[index].Height = 30;
                dgvDiem.Rows[index].Cells[0].Value = item.MaDeThi;
                dgvDiem.Rows[index].Cells[1].Value = item.MaSinhVien;
                dgvDiem.Rows[index].Cells[2].Value = item.SINH_VIEN.HoTen;
                dgvDiem.Rows[index].Cells[3].Value = item.SINH_VIEN.NgaySinh.ToString("dd/MM/yyyy");
                dgvDiem.Rows[index].Cells[4].Value = item.SINH_VIEN.Lop;
                dgvDiem.Rows[index].Cells[5].Value = item.DiemThi;
                dgvDiem.Rows[index].Cells[6].Value = item.ThoiGianVaoLam.ToString("dd/MM/yyyy _ HH:mm:ss");
                dgvDiem.Rows[index].Cells[7].Value = item.ThoiGianNop.ToString("dd/MM/yyyy _ HH:mm:ss");
            }
        }

        private void HienThiDiemTheoDiem(List<DIEM> l)
        {
            dgvDiem.Rows.Clear();

            foreach (var item in l)
            {
                int index = dgvDiem.Rows.Add();
                dgvDiem.Rows[index].Height = 30;
                dgvDiem.Rows[index].Cells[0].Value = item.MaDeThi;
                dgvDiem.Rows[index].Cells[1].Value = item.MaSinhVien;
                dgvDiem.Rows[index].Cells[2].Value = item.SINH_VIEN.HoTen;
                dgvDiem.Rows[index].Cells[3].Value = item.SINH_VIEN.NgaySinh.ToString("dd/MM/yyyy");
                dgvDiem.Rows[index].Cells[4].Value = item.SINH_VIEN.Lop;
                dgvDiem.Rows[index].Cells[5].Value = item.DiemThi;
                dgvDiem.Rows[index].Cells[6].Value = item.ThoiGianVaoLam.ToString("dd/MM/yyyy _ HH:mm:ss");
                dgvDiem.Rows[index].Cells[7].Value = item.ThoiGianNop.ToString("dd/MM/yyyy _ HH:mm:ss");
            }
        }


        private void Admin_Diem_Load(object sender, EventArgs e)
        {
            HienThiTatCaDiem();
            lbWelcome.Text = adminServices.LayTenTuMaAdminMoiDangNhap(MaAdminMoiDangNhap);

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm = new Form1();
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

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btn_TimTheoMonThiCaThi_Click(object sender, EventArgs e)
        {
            string maSV = txtTimTheoMaSV.Text;
            string tenSV = txtTimTheoTénV.Text;
            List<DIEM> list = diemThiServices.TimTheoSinhVien(maSV, tenSV);
            HienThiDiemTheoSV(list);
        }

        private void btnTimTheoDiem_Click(object sender, EventArgs e)
        {
            if ((txtDiemTu.Text == "" && txtDiemDen.Text != "") || (txtDiemDen.Text == "" && txtDiemTu.Text != ""))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ khoảng điểm", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                if (txtDiemTu.Text != null && txtDiemDen.Text != null)
                {
                    float diemtu, diemden;
                    float.TryParse(txtDiemTu.Text.ToString(), out diemtu);
                    float.TryParse(txtDiemDen.Text.ToString(), out diemden);
                    if (diemtu > diemden)
                    {
                        MessageBox.Show("Khoảng điểm không hợp lệ", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                bool TangDan = rdDiemTangDan.Checked;
                bool GiamDan = rdDiemGiamDan.Checked;
                List<DIEM> listDiem = diemThiServices.LocTheoDiem(txtDiemTu.Text, txtDiemDen.Text, TangDan, GiamDan);
                HienThiDiemTheoDiem(listDiem);
            }

            if (rdDiemLiet.Checked == true)
            {
                List<DIEM> listDiemLiet = diemThiServices.LocDiemLiet();
                HienThiDiemTheoDiem(listDiemLiet);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            HienThiTatCaDiem();
        }
        
        private void ExportExcel(string path)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Điểm Thi");

                    for (int col = 0; col < dgvDiem.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = dgvDiem.Columns[col].HeaderText;
                        worksheet.Cells[1, col + 1].Style.Font.Bold = true; // Bold for headers
                    }

                    for (int row = 0; row < dgvDiem.Rows.Count; row++)
                    {
                        for (int col = 0; col < dgvDiem.Columns.Count; col++)
                        {
                            var cellValue = dgvDiem.Rows[row].Cells[col].Value?.ToString();
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

        private void btnXuatExel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
                Title = "Save Excel File",
                FileName = "DiemThi.xlsx"
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
        
        
    }
}
