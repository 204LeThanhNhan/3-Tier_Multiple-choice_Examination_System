using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class Admin_SinhVien : Form
    {
        private string MaAdminMoiDangNhap;
        private int nutLayAnh = 0;
        private byte[] anh = null;
        public Admin_SinhVien(string ma)
        {
            InitializeComponent();
            MaAdminMoiDangNhap = ma;
        }
        private readonly AdminServices adminServices = new AdminServices();
        private readonly SinhVienServices sinhVienServices = new SinhVienServices();
        private readonly DiemThiServices diemThiServices = new DiemThiServices();
        private void ResetForm()
        {
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            txtLop.Text = "";
            txtQueQuan.Text = "";
            pictureBox1.Image = null;
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
        private void HienThiDanhSachSinhVien()
        {
            var listStudent = sinhVienServices.LayDanhSachSinhVien();
            dgvSinhVien.Rows.Clear();

            foreach (var item in listStudent)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Height = 80; // Điều chỉnh chiều cao dòng
                dgvSinhVien.Rows[index].Cells[0].Value = item.MaSinhVien;
                dgvSinhVien.Rows[index].Cells[1].Value = item.HoTen;
                dgvSinhVien.Rows[index].Cells[2].Value = item.NgaySinh.ToString("dd/MM/yyyy");
                dgvSinhVien.Rows[index].Cells[3].Value = item.QueQuan;
                dgvSinhVien.Rows[index].Cells[4].Value = item.Lop;
                dgvSinhVien.Rows[index].Cells[5].Value = ConvertByteArrayToImage(item.HinhDaiDien);
                dgvSinhVien.Rows[index].Cells[6].Value = item.UsernameSV;
                dgvSinhVien.Rows[index].Cells[7].Value = item.PasswordSV;
            }
        }
        private Image ConvertByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private byte[] ConvertImageToByteArray(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);  //jpeg, png,...
                return ms.ToArray();
            }
        }
        private void Admin_SinhVien_Load(object sender, EventArgs e)
        {
            lbWelcome.Text = adminServices.LayTenTuMaAdminMoiDangNhap(MaAdminMoiDangNhap);
            HienThiDanhSachSinhVien();
        }
        public byte[] ConvertImageToByteArray(PictureBox pictureBox)
        {
            // Kiểm tra xem PictureBox có hình ảnh không
            if (pictureBox.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Chuyển đổi hình ảnh thành byte[]
                    pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                    return ms.ToArray();
                }
            }
            return null;
        }
        private void btnThemSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSV.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtLop.Text) ||
                string.IsNullOrWhiteSpace(txtQueQuan.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    throw new Exception("Ngoài hình ảnh, Vui lòng nhập đầy đủ thông tin!!!");
                }
                //Tạo đối tượng sinh viên, lấy các thông tin từ giao diện gán vào
                SINH_VIEN s = new SINH_VIEN()
                {
                    MaSinhVien = txtMaSV.Text,
                    HoTen = txtHoTen.Text,
                    NgaySinh = dateTimePicker1.Value.Date,
                    Lop = txtLop.Text,
                    QueQuan = txtQueQuan.Text,
                    UsernameSV = txtUsername.Text,
                    PasswordSV = txtPassword.Text,
                    HinhDaiDien = ConvertImageToByteArray(pictureBox1)
                };
                if (s.HinhDaiDien == null)
                {
                    s.HinhDaiDien = ConvertImageToByteArray(Properties.Resources.HinhDaiDienMacDinh);
                }
                if (sinhVienServices.checkKyTuDacBiet(s.MaSinhVien) == true)
                {
                    throw new Exception("Mã sinh viên không được chứa ký tự đặc biệt!");
                }
                if (sinhVienServices.checkKyTuDacBiet(s.HoTen) == true ||
                    sinhVienServices.checkSo(s.HoTen) == true)
                {
                    throw new Exception("Họ tên sinh viên chỉ bao gồm chữ cái!");
                }
                if (2025 - dateTimePicker1.Value.Day < 18)
                {
                    throw new Exception("Năm sinh chưa hợp lệ!");
                }
                if (sinhVienServices.checkKyTuDacBiet(s.Lop) == true)
                {
                    throw new Exception("Lớp học chưa hợp lệ!");
                }
                if (sinhVienServices.checkKyTuDacBiet(s.QueQuan) == true)
                {
                    throw new Exception("Quê quán chưa hợp lệ!");
                }
                if (sinhVienServices.checkTonTaiSinhVien(s.MaSinhVien) == true)
                {
                    throw new Exception("Mã sinh viên này đã tồn tại! Không thể thêm");
                }

                sinhVienServices.ThemSV(s);
                MessageBox.Show("Thông tin sinh viên đã được lưu thành công!");
                HienThiDanhSachSinhVien();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        private void btnLayHinh_Click(object sender, EventArgs e)
        {
            nutLayAnh = 1;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            openFileDialog.Title = "Chọn hình ảnh";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                    anh = ConvertImageToByteArray(pictureBox1.Image);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải hình ảnh: " + ex.Message);
                }
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSV.Text = dgvSinhVien.SelectedRows[0].Cells[0].Value.ToString();
            txtHoTen.Text = dgvSinhVien.SelectedRows[0].Cells[1].Value.ToString();
            DateTime ngaySinh;
            // Kiểm tra nếu giá trị trong ô có thể chuyển thành DateTime
            if (DateTime.TryParse(dgvSinhVien.SelectedRows[0].Cells[2].Value.ToString(), out ngaySinh))
            {
                dateTimePicker1.Value = ngaySinh;  // Gán giá trị vào DateTimePicker
            }
            txtQueQuan.Text = dgvSinhVien.SelectedRows[0].Cells[3].Value.ToString();
            txtLop.Text = dgvSinhVien.SelectedRows[0].Cells[4].Value.ToString();
            // Xử lý hình ảnh
            if (dgvSinhVien.SelectedRows[0].Cells[5].Value != null)
            {

                pictureBox1.Image = ConvertByteArrayToImage(sinhVienServices.LayHinhSVTuDatabase(txtMaSV.Text));
            }
            else
            {
                pictureBox1.Image = null;
            }

            txtUsername.Text = dgvSinhVien.SelectedRows[0].Cells[6].Value.ToString();
            txtPassword.Text = dgvSinhVien.SelectedRows[0].Cells[7].Value.ToString();

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            HienThiDanhSachSinhVien();
        }

        private void btnTimTheoMaSV_Click(object sender, EventArgs e)
        {
            string MaSVDangTim = txtTimMaSV.Text;
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimMaSV.Text))
                {
                    throw new Exception("Vui lòng nhập mã sinh viên để tìm");
                }
                if (sinhVienServices.TimTheoMa(txtTimMaSV.Text) == null)
                {
                    throw new Exception("Không tìm thấy sinh viên");
                }
                else
                {
                    SINH_VIEN student = sinhVienServices.TimTheoMa(txtTimMaSV.Text);
                    dgvSinhVien.Rows.Clear();
                    int index = dgvSinhVien.Rows.Add();
                    dgvSinhVien.Rows[index].Height = 60;
                    dgvSinhVien.Rows[index].Cells[0].Value = student.MaSinhVien;
                    dgvSinhVien.Rows[index].Cells[1].Value = student.HoTen;
                    dgvSinhVien.Rows[index].Cells[2].Value = student.NgaySinh.ToString("dd/MM/yyyy");
                    dgvSinhVien.Rows[index].Cells[3].Value = student.QueQuan;
                    dgvSinhVien.Rows[index].Cells[4].Value = student.Lop;
                    dgvSinhVien.Rows[index].Cells[5].Value = ConvertByteArrayToImage(student.HinhDaiDien);
                    dgvSinhVien.Rows[index].Cells[6].Value = student.UsernameSV;
                    dgvSinhVien.Rows[index].Cells[7].Value = student.PasswordSV;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoaSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaSV.Text))
                {
                    MessageBox.Show("Vui lòng chọn 1 mã sinh viên để xóa", "Thông báo!", MessageBoxButtons.OK);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("Bạn chắc muốn xóa sinh viên này?\nNếu xóa, tất cả kết quả thi của sinh viên này cũng sẽ bị xóa", "Thông báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    //Tạo đối tượng sinh viên, lấy thông tin từ giao diện về
                    SINH_VIEN s = new SINH_VIEN()
                    {
                        MaSinhVien = txtMaSV.Text,
                        HoTen = txtHoTen.Text,
                        NgaySinh = dateTimePicker1.Value.Date,
                        Lop = txtLop.Text,
                        QueQuan = txtQueQuan.Text,
                        UsernameSV = txtUsername.Text,
                        PasswordSV = txtPassword.Text,

                    };
                    diemThiServices.XoaTatCaDiemCuaSV(s.MaSinhVien);
                    //gọi hàm delete
                    sinhVienServices.Delete(s);
                    MessageBox.Show("Xoá Sinh Viên thành công", "Thành Công!!!");
                    //gọi hàm lấy lại danh sách
                    HienThiDanhSachSinhVien();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnCapNhatSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSV.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtLop.Text) ||
                string.IsNullOrWhiteSpace(txtQueQuan.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    throw new Exception("Vui lòng cập nhập đầy đủ thông tin!!!");
                }

                if (sinhVienServices.checkKyTuDacBiet(txtMaSV.Text) == true)
                {
                    throw new Exception("Mã sinh viên không được chứa ký tự đặc biệt!");
                }
                if (sinhVienServices.checkKyTuDacBiet(txtHoTen.Text) == true ||
                    sinhVienServices.checkSo(txtHoTen.Text) == true)
                {
                    throw new Exception("Họ tên sinh viên chỉ bao gồm chữ cái!");
                }
                if (2024 - dateTimePicker1.Value.Day < 18)
                {
                    throw new Exception("Năm sinh chưa hợp lệ!");
                }
                if (sinhVienServices.checkKyTuDacBiet(txtLop.Text) == true)
                {
                    throw new Exception("Lớp học chưa hợp lệ!");
                }
                if (sinhVienServices.checkKyTuDacBiet(txtQueQuan.Text) == true)
                {
                    throw new Exception("Quê quán chưa hợp lệ!");
                }
                if (sinhVienServices.checkTonTaiSinhVien(txtMaSV.Text) == false)
                {
                    MessageBox.Show("Không tìm thấy mã SV", "Cập Nhật Thất Bại!!!");
                }
                else
                {
                    //Tạo đối tượng sinh viên, lấy các thông tin từ giao diện gán vào
                    SINH_VIEN s = new SINH_VIEN();

                    s.MaSinhVien = txtMaSV.Text;
                    s.HoTen = txtHoTen.Text;
                    s.NgaySinh = dateTimePicker1.Value.Date;
                    s.Lop = txtLop.Text;
                    s.QueQuan = txtQueQuan.Text;
                    s.UsernameSV = txtUsername.Text;
                    s.PasswordSV = txtPassword.Text;
                    if (nutLayAnh == 1 && anh != null)
                    {
                        s.HinhDaiDien = ConvertImageToByteArray(pictureBox1.Image);
                        nutLayAnh = 0;
                        anh = null;
                    }
                    else
                    {
                        s.HinhDaiDien = sinhVienServices.LayHinhSVTuDatabase(txtMaSV.Text);
                    }
                    sinhVienServices.CapNhatSV(s);
                    MessageBox.Show("Thông tin sinh viên đã được cập nhật thành công!");
                    HienThiDanhSachSinhVien();
                    ResetForm();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin frm = new Admin(MaAdminMoiDangNhap);
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

        private void btnXemDiem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_Diem frm = new Admin_Diem(MaAdminMoiDangNhap);
            frm.Show();
        }


    }
}

