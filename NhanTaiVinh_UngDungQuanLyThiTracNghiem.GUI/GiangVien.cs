using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;
using OfficeOpenXml;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class GiangVien : Form
    {
        GIANG_VIEN GiaoVienMoiDangNhap;
        public GiangVien(GIANG_VIEN GV)
        {
            InitializeComponent();
            GiaoVienMoiDangNhap = GV;
        }
        private readonly CauHoiServices cauHoiServices = new CauHoiServices();
        private readonly GiangVienServices giangVienServices = new GiangVienServices();
        private readonly DapAnServices dapAnServices = new DapAnServices();

        private void GiangVien_Load(object sender, System.EventArgs e)
        {
            txtMaGiaoVien.Text = GiaoVienMoiDangNhap.MaGiangVien;
            txtTenGiaoVien.Text = GiaoVienMoiDangNhap.TenGiangVien;
            txtSoDienThoai.Text = GiaoVienMoiDangNhap.SDT;
            txtEmail.Text = GiaoVienMoiDangNhap.Email;

            List<MON_HOC> listMonHoc = cauHoiServices.LayMonThi();
            this.cbMonHoc.DataSource = listMonHoc;
            this.cbMonHoc.DisplayMember = "TenMon";
            this.cbMonHoc.ValueMember = "MaMon";

            cbDoKho.Text = "";
            monDangchon = cbMonHoc.SelectedValue?.ToString();
            HienThiDanhSachCauHoi();
        }

        private void HienThiDanhSachCauHoi()
        {
            List<CAU_HOI> list = cauHoiServices.LayThongTinCauHoiTheoMaMonHocVaMaGV(monDangchon, GiaoVienMoiDangNhap.MaGiangVien);
            dgvCauHoi.Rows.Clear();
            foreach (var item in list)
            {
                int index = dgvCauHoi.Rows.Add();
                dgvCauHoi.Rows[index].Height = 30; // Điều chỉnh chiều cao dòng
                dgvCauHoi.Rows[index].Cells[0].Value = item.MaCauHoi;
                dgvCauHoi.Rows[index].Cells[1].Value = item.NoiDung;
                //dgvCauHoi.Rows[index].Cells[2].Value = item.MaGiangVien;
                //dgvCauHoi.Rows[index].Cells[3].Value = giangVienServices.LayTenGVTheoMaGV(item.MaGiangVien);
                dgvCauHoi.Rows[index].Cells[2].Value = dapAnServices.LayNoiDungDapAnATheoMaCauHoi(item.MaCauHoi);
                dgvCauHoi.Rows[index].Cells[3].Value = dapAnServices.LayNoiDungDapAnBTheoMaCauHoi(item.MaCauHoi);
                dgvCauHoi.Rows[index].Cells[4].Value = dapAnServices.LayNoiDungDapAnCTheoMaCauHoi(item.MaCauHoi);
                dgvCauHoi.Rows[index].Cells[5].Value = dapAnServices.LayNoiDungDapAnDTheoMaCauHoi(item.MaCauHoi);
                dgvCauHoi.Rows[index].Cells[6].Value = dapAnServices.DapAnDungTheoMaCauHoi(item.MaCauHoi);
                dgvCauHoi.Rows[index].Cells[7].Value = item.DoKho;
            }
        }

        private void btnThoat_Click(object sender, System.EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Cảnh báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.Close();
                Form1 frmDangNhap = new Form1();
                frmDangNhap.Show();
            }
        }
        private void ResetForm()
        {
            btnLuu.Enabled = true;
            rtbSoanCauHoi.Text = "";
            txtA.Text = "";
            txtB.Text = "";
            txtC.Text = "";
            txtD.Text = "";
            rdDapAnA.Checked = true;
            rdDapAnB.Checked = false;
            rdDapAnC.Checked = false;
            rdDapAnD.Checked = false;
            cbDoKho.Text = "";

            rtbXemNoiDung.Text = "";
        }
        private void btnTaiLai_Click(object sender, System.EventArgs e)
        {

            ResetForm();
            HienThiDanhSachCauHoi();
        }
        private string monDangchon;
        private void cbMonHoc_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string MonDangChon = cbMonHoc.SelectedValue.ToString();
            monDangchon = MonDangChon;
            HienThiDanhSachCauHoi();
        }

        private void btnLuu_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (cbMonHoc.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn môn học!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (rtbSoanCauHoi.Text == "")
                {
                    MessageBox.Show("Bạn chưa soạn nội dung cho câu hỏi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtA.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập nội dung đáp án A!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtB.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập nội dung đáp án B!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtC.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập nội dung đáp án C!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtD.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập nội dung đáp án D!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cbDoKho.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn độ khó cho câu hỏi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int stt = cauHoiServices.LaySTTCauHoi(monDangchon) + 1; //+1 để index câu hỏi bắt đầu = 1
                CAU_HOI CauHoiMoi = new CAU_HOI();
                string maCauHoi = monDangchon + stt.ToString();
                string noiDung = rtbSoanCauHoi.Text;
                string doKho = cbDoKho.Text;
                string maMon = monDangchon;
                string maGV = GiaoVienMoiDangNhap.MaGiangVien;
                CauHoiMoi = cauHoiServices.TaoCauHoi(maCauHoi, noiDung, doKho, maMon, maGV);
                cauHoiServices.ThemCauHoi(CauHoiMoi);


                DAP_AN A = new DAP_AN();
                string MaDapAn = maCauHoi + "A";
                string MaCauHoi_DapAn = maCauHoi;
                string MaGiangVienDapAn = GiaoVienMoiDangNhap.MaGiangVien;
                string MaMon_DapAn = maMon;
                string NoiDungDapAn = txtA.Text;
                bool DungSai = rdDapAnA.Checked;
                A = cauHoiServices.TaoDapAn(MaDapAn, MaCauHoi_DapAn, MaGiangVienDapAn, MaMon_DapAn, NoiDungDapAn, DungSai);
                cauHoiServices.ThemDapAn(A);

                DAP_AN B = new DAP_AN();
                string MaDapAnB = maCauHoi + "B";
                string MaCauHoi_DapAnB = maCauHoi;
                string MaGiangVienDapAnB = GiaoVienMoiDangNhap.MaGiangVien;
                string MaMon_DapAnB = maMon;
                string NoiDungDapAnB = txtB.Text;
                bool DungSaiB = rdDapAnB.Checked;
                B = cauHoiServices.TaoDapAn(MaDapAnB, MaCauHoi_DapAnB, MaGiangVienDapAnB, MaMon_DapAnB, NoiDungDapAnB, DungSaiB);
                cauHoiServices.ThemDapAn(B);

                DAP_AN C = new DAP_AN();
                string MaDapAnC = maCauHoi + "C";
                string MaCauHoi_DapAnC = maCauHoi;
                string MaGiangVienDapAnC = GiaoVienMoiDangNhap.MaGiangVien;
                string MaMon_DapAnC = maMon;
                string NoiDungDapAnC = txtC.Text;
                bool DungSaiC = rdDapAnC.Checked;
                C = cauHoiServices.TaoDapAn(MaDapAnC, MaCauHoi_DapAnC, MaGiangVienDapAnC, MaMon_DapAnC, NoiDungDapAnC, DungSaiC);
                cauHoiServices.ThemDapAn(C);

                DAP_AN D = new DAP_AN();
                string MaDapAnD = maCauHoi + "D";
                string MaCauHoi_DapAnD = maCauHoi;
                string MaGiangVienDapAnD = GiaoVienMoiDangNhap.MaGiangVien;
                string MaMon_DapAnD = maMon;
                string NoiDungDapAnD = txtD.Text;
                bool DungSaiD = rdDapAnD.Checked;
                D = cauHoiServices.TaoDapAn(MaDapAnD, MaCauHoi_DapAnD, MaGiangVienDapAnD, MaMon_DapAnD, NoiDungDapAnD, DungSaiD);
                cauHoiServices.ThemDapAn(D);

                MessageBox.Show("Thêm câu hỏi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachCauHoi();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cbDoKho_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void SetDapAn(string kytu)
        {
            string DapAnDung = kytu;
            if (DapAnDung == "A")
            {
                rdDapAnA.Checked = true;
            }
            else if (DapAnDung == "B")
            {
                rdDapAnB.Checked = true;
            }
            else if (DapAnDung == "C")
            {
                rdDapAnC.Checked = true;
            }
            else
            {
                rdDapAnD.Checked = true;
            }
        }
        private void dgvCauHoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnLuu.Enabled = false;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvCauHoi.Rows[e.RowIndex].Selected = true;

                // Lấy giá trị của ô được click
                string cellValue = dgvCauHoi.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                rtbXemNoiDung.Text = cellValue;
            }

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCauHoi.Rows[e.RowIndex];

                rtbSoanCauHoi.Text = row.Cells[1].Value?.ToString();
                txtA.Text = row.Cells[2].Value?.ToString();
                txtB.Text = row.Cells[3].Value?.ToString();
                txtC.Text = row.Cells[4].Value?.ToString();
                txtD.Text = row.Cells[5].Value?.ToString();
                cbDoKho.Text = row.Cells[7].Value?.ToString();
            }
            SetDapAn(dgvCauHoi.SelectedRows[0].Cells[6].Value.ToString());
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMonHoc.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn môn học!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                List<string> listMaCauHoiCanXoa = new List<string>();
                foreach (DataGridViewRow row in dgvCauHoi.SelectedRows)
                {
                    string maCauHoi = row.Cells[0].Value?.ToString();
                    if (!string.IsNullOrEmpty(maCauHoi))
                    {
                        listMaCauHoiCanXoa.Add(maCauHoi);
                    }
                }
                DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn xóa (" + listMaCauHoiCanXoa.Count + ") câu hỏi?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    dapAnServices.XoaDapAnTheoMaCauHoi(listMaCauHoiCanXoa);
                    cauHoiServices.XoaCauHoiTheoMaCauHoi(listMaCauHoiCanXoa);
                    MessageBox.Show("Xóa câu hỏi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachCauHoi();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            try
            {
                if (string.IsNullOrEmpty(rtbSoanCauHoi.Text))
                {
                    MessageBox.Show("Bạn chưa chọn câu hỏi để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (string.IsNullOrEmpty(txtA.Text))
                {
                    MessageBox.Show("Bạn nhập còn thiếu đáp án A", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (string.IsNullOrEmpty(txtB.Text))
                {
                    MessageBox.Show("Bạn nhập còn thiếu đáp án B", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (string.IsNullOrEmpty(txtC.Text))
                {
                    MessageBox.Show("Bạn nhập còn thiếu đáp án C", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (string.IsNullOrEmpty(txtD.Text))
                {
                    MessageBox.Show("Bạn nhập còn thiếu đáp án D", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                string MaCauHoiCanSua = dgvCauHoi.SelectedRows[0].Cells[0].Value.ToString();
                cauHoiServices.SuaCauHoiTheoMa(MaCauHoiCanSua, rtbSoanCauHoi.Text, cbDoKho.Text);
                dapAnServices.SuaDapAnATheoMaCauHoi(MaCauHoiCanSua, txtA.Text, rdDapAnA.Checked);
                dapAnServices.SuaDapAnBTheoMaCauHoi(MaCauHoiCanSua, txtB.Text, rdDapAnB.Checked);
                dapAnServices.SuaDapAnCTheoMaCauHoi(MaCauHoiCanSua, txtC.Text, rdDapAnC.Checked);
                dapAnServices.SuaDapAnDTheoMaCauHoi(MaCauHoiCanSua, txtD.Text, rdDapAnD.Checked);
                MessageBox.Show("Cập nhật câu hỏi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachCauHoi();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void HienThiDanhSachCauHoiSauTimKiem()
        {
            List<CAU_HOI> list = cauHoiServices.TimCauHoi(GiaoVienMoiDangNhap.MaGiangVien, rtbTimCauHoi.Text, monDangchon);
            if (list != null)
            {
                dgvCauHoi.Rows.Clear();
                foreach (var item in list)
                {
                    int index = dgvCauHoi.Rows.Add();
                    dgvCauHoi.Rows[index].Height = 30; // Điều chỉnh chiều cao dòng
                    dgvCauHoi.Rows[index].Cells[0].Value = item.MaCauHoi;
                    dgvCauHoi.Rows[index].Cells[1].Value = item.NoiDung;
                    //dgvCauHoi.Rows[index].Cells[2].Value = item.MaGiangVien;
                    //dgvCauHoi.Rows[index].Cells[3].Value = giangVienServices.LayTenGVTheoMaGV(item.MaGiangVien);
                    dgvCauHoi.Rows[index].Cells[2].Value = dapAnServices.LayNoiDungDapAnATheoMaCauHoi(item.MaCauHoi);
                    dgvCauHoi.Rows[index].Cells[3].Value = dapAnServices.LayNoiDungDapAnBTheoMaCauHoi(item.MaCauHoi);
                    dgvCauHoi.Rows[index].Cells[4].Value = dapAnServices.LayNoiDungDapAnCTheoMaCauHoi(item.MaCauHoi);
                    dgvCauHoi.Rows[index].Cells[5].Value = dapAnServices.LayNoiDungDapAnDTheoMaCauHoi(item.MaCauHoi);
                    dgvCauHoi.Rows[index].Cells[6].Value = dapAnServices.DapAnDungTheoMaCauHoi(item.MaCauHoi);
                    dgvCauHoi.Rows[index].Cells[7].Value = item.DoKho;
                }
            }
            else
            {
                MessageBox.Show("Câu hỏi có mã và nội dung theo yêu cầu không tồn tại!");
            }

        }
        private void btnTimCauHoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(rtbTimCauHoi.Text))
                {
                    MessageBox.Show("Bạn chưa nhập thông tin của câu hỏi cần tìm", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                HienThiDanhSachCauHoiSauTimKiem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ExportExcel(string path)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Điểm Thi");

                    for (int col = 0; col < dgvCauHoi.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = dgvCauHoi.Columns[col].HeaderText;
                        worksheet.Cells[1, col + 1].Style.Font.Bold = true; // Bold for headers
                    }

                    for (int row = 0; row < dgvCauHoi.Rows.Count; row++)
                    {
                        for (int col = 0; col < dgvCauHoi.Columns.Count; col++)
                        {
                            var cellValue = dgvCauHoi.Rows[row].Cells[col].Value?.ToString();
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
                FileName = "CauHoi.xlsx"
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
                        //tao cau hoi
                        int stt = cauHoiServices.LaySTTCauHoi(monDangchon) + 1; //+1 để index câu hỏi bắt đầu = 1
                        CAU_HOI CauHoiMoi = new CAU_HOI();
                        CauHoiMoi.MaCauHoi = monDangchon + stt.ToString();
                        CauHoiMoi.NoiDung = worksheet.Cells[row, 1].Text.Trim();
                        CauHoiMoi.DoKho = worksheet.Cells[row, 7].Text.Trim();
                        CauHoiMoi.MaMon = monDangchon;
                        CauHoiMoi.MaGiangVien = GiaoVienMoiDangNhap.MaGiangVien;
                        cauHoiServices.ThemCauHoi(CauHoiMoi);

                        //tao dap an
                        DAP_AN A = new DAP_AN();
                        A.MaDapAN = CauHoiMoi.MaCauHoi + "A";
                        A.MaCauHoi = CauHoiMoi.MaCauHoi;
                        A.MaGiangVien = GiaoVienMoiDangNhap.MaGiangVien;
                        A.MaMon = monDangchon;
                        A.NoiDungDapAn = worksheet.Cells[row, 2].Text.Trim();
                        A.DungSai = false;

                        DAP_AN B = new DAP_AN();
                        B.MaDapAN = CauHoiMoi.MaCauHoi + "B";
                        B.MaCauHoi = CauHoiMoi.MaCauHoi;
                        B.MaGiangVien = GiaoVienMoiDangNhap.MaGiangVien;
                        B.MaMon = monDangchon;
                        B.NoiDungDapAn = worksheet.Cells[row, 3].Text.Trim();
                        B.DungSai = false;

                        DAP_AN C = new DAP_AN();
                        C.MaDapAN = CauHoiMoi.MaCauHoi + "C";
                        C.MaCauHoi = CauHoiMoi.MaCauHoi;
                        C.MaGiangVien = GiaoVienMoiDangNhap.MaGiangVien;
                        C.MaMon = monDangchon;
                        C.NoiDungDapAn = worksheet.Cells[row, 4].Text.Trim();
                        C.DungSai = false;

                        DAP_AN D = new DAP_AN();
                        D.MaDapAN = CauHoiMoi.MaCauHoi + "D";
                        D.MaCauHoi = CauHoiMoi.MaCauHoi;
                        D.MaGiangVien = GiaoVienMoiDangNhap.MaGiangVien;
                        D.MaMon = monDangchon;
                        D.NoiDungDapAn = worksheet.Cells[row, 5].Text.Trim();
                        D.DungSai = false;

                        string DapAnDung = worksheet.Cells[row, 6].Text.Trim();
                        if(DapAnDung == "A")
                        {
                            A.DungSai = true;
                        }else if(DapAnDung == "B")
                        {
                            B.DungSai = true;
                        }else if (DapAnDung == "C")
                        {
                            C.DungSai = true;
                        }
                        else
                        {
                            D.DungSai = true;
                        }


                        cauHoiServices.ThemDapAn(A);
                        cauHoiServices.ThemDapAn(B);
                        cauHoiServices.ThemDapAn(C);
                        cauHoiServices.ThemDapAn(D);
                    }
                }
                MessageBox.Show("Nhập liệu thành công", "Thành Công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachCauHoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnLuuExcel_Click(object sender, EventArgs e)
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
