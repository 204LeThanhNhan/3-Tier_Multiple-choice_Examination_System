using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class Admin_CaThi : Form
    {
        string MaAdminMoiDangNhap;
        public Admin_CaThi(string ma)
        {
            InitializeComponent();
            MaAdminMoiDangNhap = ma;
        }


        private readonly DeThiServices deThiServices = new DeThiServices();
        private readonly CaThiServices caThiServices = new CaThiServices();
        private readonly MonHocServices monHocServices = new MonHocServices();
        private readonly CauHoiServices cauHoiServices = new CauHoiServices();
        private readonly AdminServices adminServices = new AdminServices();
        private void HienThiDanhSachCaThi()
        {
            dgvCaThi.Rows.Clear();
            List<CA_THI> list = caThiServices.LayDanhSachCaThi();
            dgvCaThi.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";
            foreach (var item in list)
            {

                int index = dgvCaThi.Rows.Add();
                dgvCaThi.Rows[index].Height = 20;
                dgvCaThi.Rows[index].Cells[0].Value = item.MaCaThi;
                dgvCaThi.Rows[index].Cells[1].Value = item.NgayCaThi;
                dgvCaThi.Rows[index].Cells[2].Value = item.MaMon;
                dgvCaThi.Rows[index].Cells[3].Value = monHocServices.TimTenMonHocTheoMaMon(item.MaMon);
                dgvCaThi.Rows[index].Cells[4].Value = item.GioBatDau;
                dgvCaThi.Rows[index].Cells[5].Value = deThiServices.LayThoiGianLamBaiTheoMaCaThi(item.MaCaThi) + " phút";
            }
        }
        private void Admin_CaThi_Load(object sender, EventArgs e)
        {
            lbWelcome.Text = adminServices.LayTenTuMaAdminMoiDangNhap(MaAdminMoiDangNhap);

            HienThiDanhSachCaThi();

            List<MON_HOC> listMonHoc = monHocServices.LayDanhSachMonHoc();
            this.cbMonThi.DataSource = listMonHoc;
            this.cbMonThi.DisplayMember = "TenMon";
            this.cbMonThi.ValueMember = "MaMon";
        }
        private void ResetForm()
        {
            txtMaCaThi.Text = "";
            txtSoPhut.Text = "";
            cbGioThi.Text = "";
            txtTongSoCau.Text = "";
            txtSoCauDe.Text = "";
            txtSoCauTB.Text = "";
            txtSoCauKho.Text = "";
            txtTimMonThi.Text = "";
        }
        private void btnTaiLaiTrang_Click(object sender, EventArgs e)
        {

            ResetForm();
            HienThiDanhSachCaThi();
        }

        private void txtTimMonThi_TextChanged(object sender, EventArgs e)
        {

            if (txtTimMonThi.Text == "")
            {
                List<MON_HOC> listMonHoc = monHocServices.LayDanhSachMonHoc();
                this.cbMonThi.DataSource = listMonHoc;
                this.cbMonThi.DisplayMember = "TenMon";
                this.cbMonThi.ValueMember = "MaMon";
            }
            else
            {
                List<MON_HOC> listMonHoc = monHocServices.TimKiemMonHoc(txtTimMonThi.Text);
                this.cbMonThi.DataSource = listMonHoc;
                this.cbMonThi.DisplayMember = "TenMon";
                this.cbMonThi.ValueMember = "MaMon";
            }

        }

        private void btnXoaCaThi_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrEmpty(txtMaCaThi.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã ca thi", "Chưa chọn mã ca thi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (caThiServices.KiemTraTruocKhiThaoTacCaThi(txtMaCaThi.Text) == false)
                {
                    MessageBox.Show("Không thể xóa được ca thi\nKết quả của một số sinh viên thi trong ca này còn được lưu", "Không thể xóa ca thi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    caThiServices.XoaCaThi(txtMaCaThi.Text);
                    MessageBox.Show("Xóa ca thi thành công", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachCaThi();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCaThi_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            txtMaCaThi.Text = dgvCaThi.SelectedRows[0].Cells[0].Value.ToString();
            dateTimePicker1.Value = (DateTime)dgvCaThi.SelectedRows[0].Cells[1].Value;
            cbMonThi.Text = dgvCaThi.SelectedRows[0].Cells[3].Value.ToString();
            txtSoPhut.Text = deThiServices.LayThoiGianLamBaiTheoMaCaThi(dgvCaThi.SelectedRows[0].Cells[0].Value.ToString()).ToString();
            cbGioThi.Text = dgvCaThi.SelectedRows[0].Cells[4].Value.ToString();
            txtTongSoCau.Text = deThiServices.LayDeThiTheoMaCaThi(dgvCaThi.SelectedRows[0].Cells[0].Value.ToString()).SoLuongCauHoi.ToString();
            txtSoCauDe.Text = deThiServices.LayDeThiTheoMaCaThi(dgvCaThi.SelectedRows[0].Cells[0].Value.ToString()).SoLuongDe.ToString();
            txtSoCauTB.Text = deThiServices.LayDeThiTheoMaCaThi(dgvCaThi.SelectedRows[0].Cells[0].Value.ToString()).SoLuongVua.ToString();
            txtSoCauKho.Text = deThiServices.LayDeThiTheoMaCaThi(dgvCaThi.SelectedRows[0].Cells[0].Value.ToString()).SoLuongKho.ToString();
        }

        private void btnThemCaThi_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaCaThi.Text))
                {
                    MessageBox.Show("Bạn chưa nhập mã ca thi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (caThiServices.checkNgayCaThiSauNgayHeThong(dateTimePicker1.Value.Date) == false)
                {
                    MessageBox.Show("Ngày ca thi không thể trước ngày hiện tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (caThiServices.checkNgayCaThiBangNgayHeThong(dateTimePicker1.Value.Date))
                {
                    if (caThiServices.checkGioCaThi(cbGioThi.Text) == false)
                    {
                        MessageBox.Show("Giờ bắt đầu ca thi không thể sớm hơn giờ hiện tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (int.Parse(txtSoPhut.Text) < 15 || int.Parse(txtSoPhut.Text) > 90)
                {
                    MessageBox.Show("Thời gian làm bài không hợp lệ\nThời gian làm bài trong khoảng 90 phút", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(cbGioThi.Text))
                {
                    MessageBox.Show("Vui lòng nhập giờ bắt đầu ca thi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(txtTongSoCau.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số câu hỏi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtTongSoCau.Text))
                    {
                        MessageBox.Show("Số lượng câu trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtSoCauDe.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số lượng câu hỏi dễ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiDeCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtSoCauDe.Text))
                    {
                        MessageBox.Show("Số câu dễ trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtSoCauTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số lượng câu hỏi trung bình", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiTrungBinhCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtSoCauTB.Text))
                    {
                        MessageBox.Show("Số câu trung bình trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtSoCauKho.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số lượng câu hỏi khó", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiKhoCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtSoCauKho.Text))
                    {
                        MessageBox.Show("Số câu khó trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (int.Parse(txtSoCauDe.Text) + int.Parse(txtSoCauTB.Text) + int.Parse(txtSoCauKho.Text) < int.Parse(txtTongSoCau.Text)
                    ||
                    int.Parse(txtSoCauDe.Text) + int.Parse(txtSoCauTB.Text) + int.Parse(txtSoCauKho.Text) > int.Parse(txtTongSoCau.Text))
                {
                    MessageBox.Show("Tổng số lượng các câu hỏi dễ, trung bình và khó phải bằng tổng số câu hỏi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (caThiServices.checkTonTaiMaCaThi(txtMaCaThi.Text))
                {
                    MessageBox.Show("Mã ca thi này đã tồn tại!Vui lòng nhập mã khác", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                CA_THI ca_thi = new CA_THI();
                ca_thi.MaCaThi = txtMaCaThi.Text;
                ca_thi.NgayCaThi = dateTimePicker1.Value.Date;
                TimeSpan gbd = TimeSpan.ParseExact(cbGioThi.Text, "hh\\:mm", System.Globalization.CultureInfo.InvariantCulture);
                ca_thi.GioBatDau = gbd;
                int SoPhut = int.Parse(txtSoPhut.Text);
                ca_thi.GioKetThuc = gbd.Add(TimeSpan.FromMinutes(SoPhut));
                ca_thi.MaMon = cbMonThi.SelectedValue.ToString();
                caThiServices.ThemCaThi(ca_thi);
                // tao de thi
                deThiServices.TaoDeThi(txtMaCaThi.Text, int.Parse(txtSoPhut.Text), int.Parse(txtTongSoCau.Text),
                                        int.Parse(txtSoCauDe.Text), int.Parse(txtSoCauTB.Text), int.Parse(txtSoCauKho.Text), false);
                MessageBox.Show("Tạo Ca Thi thành công", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachCaThi();
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

        private void btnCapNhatCaThi_Click(object sender, EventArgs e)
        {
            try
            {
                if(caThiServices.KiemTraTruocKhiThaoTacCaThi(txtMaCaThi.Text) == false)
                {
                    MessageBox.Show("Không thể cập nhật ca thi\n Đã có kết quả sinh viên thi trong ca này", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (caThiServices.checkNgayCaThiSauNgayHeThong(dateTimePicker1.Value.Date) == false)
                {
                    MessageBox.Show("Ngày ca thi không thể trước ngày hiện tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (caThiServices.checkNgayCaThiBangNgayHeThong(dateTimePicker1.Value.Date))
                {
                    if (caThiServices.checkGioCaThi(cbGioThi.Text) == false)
                    {
                        MessageBox.Show("Giờ bắt đầu ca thi không thể sớm hơn giờ hiện tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (int.Parse(txtSoPhut.Text) < 15 || int.Parse(txtSoPhut.Text) > 90)
                {
                    MessageBox.Show("Thời gian làm bài không hợp lệ\nThời gian làm bài trong khoảng 90 phút", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(cbGioThi.Text))
                {
                    MessageBox.Show("Vui lòng nhập giờ bắt đầu ca thi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(txtTongSoCau.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số câu hỏi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtTongSoCau.Text))
                    {
                        MessageBox.Show("Số lượng câu trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtSoCauDe.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số lượng câu hỏi dễ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiDeCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtSoCauDe.Text))
                    {
                        MessageBox.Show("Số câu dễ trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtSoCauTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số lượng câu hỏi trung bình", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiTrungBinhCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtSoCauTB.Text))
                    {
                        MessageBox.Show("Số câu trung bình trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtSoCauKho.Text))
                {
                    MessageBox.Show("Vui lòng nhập tổng số lượng câu hỏi khó", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (cauHoiServices.TongSoCauHoiKhoCuaMon(cbMonThi.SelectedValue.ToString()) < int.Parse(txtSoCauKho.Text))
                    {
                        MessageBox.Show("Số câu khó trong ngân hàng câu hỏi không đủ đáp ứng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (int.Parse(txtSoCauDe.Text) + int.Parse(txtSoCauTB.Text) + int.Parse(txtSoCauKho.Text) < int.Parse(txtTongSoCau.Text)
                    ||
                    int.Parse(txtSoCauDe.Text) + int.Parse(txtSoCauTB.Text) + int.Parse(txtSoCauKho.Text) > int.Parse(txtTongSoCau.Text))
                {
                    MessageBox.Show("Tổng số lượng các câu hỏi dễ, trung bình và khó phải bằng tổng số câu hỏi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (caThiServices.checkTonTaiMaCaThi(txtMaCaThi.Text) == false)
                {
                    MessageBox.Show("Không thể cập nhật!\nMã ca thi không tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    //cap nhat ca thi
                    DateTime NgayMoi = dateTimePicker1.Value.Date;
                    TimeSpan GioBDMoi = TimeSpan.Parse(cbGioThi.Text);
                    TimeSpan GioKTMoi = GioBDMoi.Add(TimeSpan.FromMinutes(double.Parse(txtSoPhut.Text)));
                    string MaMonMoi = cbMonThi.SelectedValue.ToString();
                    caThiServices.CapNhatCaThi(txtMaCaThi.Text, NgayMoi, GioBDMoi, GioKTMoi, MaMonMoi);
                    //cap nhat de thi
                    int tglb = int.Parse(txtSoPhut.Text);
                    int slch = int.Parse(txtTongSoCau.Text);
                    int sld = int.Parse(txtSoCauDe.Text);
                    int slv = int.Parse(txtSoCauTB.Text);
                    int slk = int.Parse(txtSoCauKho.Text);
                    deThiServices.CapNhatDeThiTheoMaCaThi(txtMaCaThi.Text,tglb,slch,sld,slv,slk);
                    HienThiDanhSachCaThi();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCaThi_Click(object sender, EventArgs e)
        {

        }

        private void btnXemDiem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_Diem frm = new Admin_Diem(MaAdminMoiDangNhap);
            frm.Show();
        }
    }
}


