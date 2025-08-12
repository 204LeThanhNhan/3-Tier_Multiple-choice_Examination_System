using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.BUS;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    public partial class ThucHienThi : Form
    {
        SINH_VIEN SVMoiDangNhap;
        DE_THI DeThiVuaChon;
        private int ThoiGianConLai; // Đơn vị giây
        private DataTable dtDapAnDaChon = null;
        private bool isLoaded = false;
        private DateTime GioVaoThi;
        private DateTime GioKetThuc;
        public ThucHienThi(SINH_VIEN s, DE_THI d)
        {
            InitializeComponent();
            this.SVMoiDangNhap = s;
            DeThiVuaChon = d;
        }
        private readonly MonHocServices monHocServices = new MonHocServices();
        private readonly DeThiServices deThiServices = new DeThiServices();
        private readonly CauHoiServices cauHoiServices = new CauHoiServices();
        private readonly CTDeThiServices cTDeThiServices = new CTDeThiServices();
        private readonly DiemThiServices diemThiServices = new DiemThiServices();
        private readonly SinhVienServices sinhVienServices = new SinhVienServices();
        private Image ConvertByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
        private async Task<DateTime> LayNgayGio()
        {
            DateTime test = await sinhVienServices.LayNgayGioTuAPI();
            return test;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ThoiGianConLai--;

            // Cập nhật thời gian lên lbThoiGian
            CapNhatThoiGian();

            // Kiểm tra nếu hết giờ
            if (ThoiGianConLai <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Đã hết thời gian làm bài!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LuuDapAnHienTai();


                float Diem = 0;
                float DiemMoiCau = (float)(10.0 / (DeThiVuaChon.SoLuongCauHoi * 1.0));
                float SoCauDung = 0;
                foreach (DataRow row in dtDapAnDaChon.Rows)
                {
                    string DapAnDung = row["DapAnDung"].ToString().Trim();
                    string DapAnSinhVienChon = row["DapAnDaChon"].ToString().Trim();
                    if (DapAnSinhVienChon.Equals(DapAnDung))
                    {
                        SoCauDung++;
                    }
                }
                Diem = (float)Math.Round(SoCauDung * DiemMoiCau, 1);
                string tenmonthi = lbTenMonThi.Text;
                //MessageBox.Show($"Bạn được {Diem}\nĐiểm mỗi câu: {DiemMoiCau}");

                //truyen sv, de, diem qua form KetQuaThi
                this.Close();
                KetQuaThi frm = new KetQuaThi(SVMoiDangNhap, DeThiVuaChon, tenmonthi, Diem, SoCauDung);
                frm.Show();
            }
        }
        private int Index = 0;
        private void CapNhatThoiGian()
        {
            int phut = ThoiGianConLai / 60;
            int giay = ThoiGianConLai % 60;
            lbThoiGian.Text = $"{phut:D2}:{giay:D2}";  // Hiển thị dạng mm:ss
        }

        private async void LayGioBatDau()
        {
            GioVaoThi = await LayNgayGio();
        }
        private void ThucHienThi_Load(object sender, EventArgs e)
        {
            LayGioBatDau();
            pictureBox1.Image = ConvertByteArrayToImage(SVMoiDangNhap.HinhDaiDien);
            lbMaSV.Text = SVMoiDangNhap.MaSinhVien.ToString();
            lbHoTen.Text = SVMoiDangNhap.HoTen.ToString();
            lbNgaySinh.Text = SVMoiDangNhap.NgaySinh.ToString("dd/MM/yyyy");
            lbLop.Text = SVMoiDangNhap.Lop.ToString();
            lbQuequan.Text = SVMoiDangNhap.QueQuan.ToString();
            lbMaDe.Text = DeThiVuaChon.MaDeThi.ToString();
            lbTenMonThi.Text = deThiServices.TimTenMonHocTheoMaDeThi(DeThiVuaChon);
            lbSoCauHoi.Text = DeThiVuaChon.SoLuongCauHoi.ToString();




            LoadDuLieuCauHoilvDSCauHoi();



            // Chuyển thời gian làm bài (số phút) thành giây
            ThoiGianConLai = DeThiVuaChon.ThoiGianLamBai * 60;

            // Hiển thị thời gian ban đầu
            CapNhatThoiGian();
            isLoaded = true;
        }

        private void LoadDuLieuCauHoilvDSCauHoi()
        {
            try
            {
                //Tat su kien SelectedIndexChanged
                lvDSCauHoi.SelectedIndexChanged -= lvDSCauHoi_SelectedIndexChanged;

                string MaMon = deThiServices.TimMaMonHocTheoMaDeThi(DeThiVuaChon);
                int sld = DeThiVuaChon.SoLuongDe;
                int slv = DeThiVuaChon.SoLuongVua;
                int slk = DeThiVuaChon.SoLuongKho;


                dtDapAnDaChon = deThiServices.RandomCauHoiChoDeThi(MaMon, sld, slv, slk);
                //them 1 cot moi cho dtDapAnDaChon giup luu nhung dap an do SV chon
                dtDapAnDaChon.Columns.Add("DapAnDaChon");
                lvDSCauHoi.DataSource = dtDapAnDaChon;
                lvDSCauHoi.DisplayMember = "QuestionIndex";
                lvDSCauHoi.ValueMember = "MaCauHoi";




                // Mo lai su kien SelectedIndexChanged
                lvDSCauHoi.SelectedIndexChanged += lvDSCauHoi_SelectedIndexChanged;

                Index = 0;
                lvDSCauHoi.SelectedIndex = Index;

                HienThiChiTietCauHoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void LuuVaHienThiDapAnDaChon()
        {
            string dapAnChonTruocDo = dtDapAnDaChon.Rows[Index]["DapAnDaChon"].ToString();

            if (rtbDapAnA.Text.Equals(dapAnChonTruocDo))
            {
                rdA.Checked = true;
            }
            else if (rtbDapAnB.Text.Equals(dapAnChonTruocDo))
            {
                rdB.Checked = true;
            }
            else if (rtbDapAnC.Text.Equals(dapAnChonTruocDo))
            {
                rdC.Checked = true;
            }
            else if (rtbDapAnD.Text.Equals(dapAnChonTruocDo))
            {
                rdD.Checked = true;
            }
        }


        private string LayNoiDungDapAnChon()
        {
            string dapan = string.Empty;
            if (rdA.Checked == true)
            {
                dapan = rtbDapAnA.Text;
            }
            else if (rdB.Checked == true)
            {
                dapan = rtbDapAnB.Text;
            }
            else if (rdC.Checked == true)
            {
                dapan = rtbDapAnC.Text;
            }
            else if (rdD.Checked == true)
            {
                dapan = rtbDapAnD.Text;
            }
            return dapan;
        }

        private void HienThiChiTietCauHoi()
        {
            try
            {
                // Lưu đáp án đã chọn
                if (Index >= 0 && Index < dtDapAnDaChon.Rows.Count)
                {
                    dtDapAnDaChon.Rows[Index]["DapAnDaChon"] = LayNoiDungDapAnChon();
                }

                // Cập nhật chỉ số câu hỏi mới
                Index = lvDSCauHoi.SelectedIndex;
                lbCauHoi.Text = lvDSCauHoi.Text;

                //ẩn/hiện các nút chuyển câu hỏi
                if (Index < DeThiVuaChon.SoLuongCauHoi - 1)
                {
                    btnCauSau.Enabled = true;
                }
                else
                {
                    btnCauSau.Enabled = false;
                }
                if (Index > 0)
                {
                    btnCauTruoc.Enabled = true;
                }
                else
                {
                    btnCauTruoc.Enabled = false;
                }


                // Lấy nội dung câu hỏi mới
                string MaCauHoiVuaChon = lvDSCauHoi.SelectedValue.ToString();
                CAU_HOI_IN_EXAM CauVuaChon = deThiServices.LayMotCauHoiTrongDeThi(MaCauHoiVuaChon);

                // Hiển thị câu hỏi và đáp án
                rtbCauHoi.Text = CauVuaChon.NoiDung.ToString();
                rtbDapAnA.Text = CauVuaChon.DapAnA.ToString();
                rtbDapAnB.Text = CauVuaChon.DapAnB.ToString();
                rtbDapAnC.Text = CauVuaChon.DapAnC.ToString();
                rtbDapAnD.Text = CauVuaChon.DapAnD.ToString();

                //reset cac lua chon
                rdA.Checked = rdB.Checked = rdC.Checked = rdD.Checked = false;

                // Hiển thị đáp án đã chọn trước đó
                LuuVaHienThiDapAnDaChon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvDSCauHoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded)
            {
                if (lvDSCauHoi.SelectedIndex == Index)
                {
                    return;
                }
                HienThiChiTietCauHoi();
            }

        }

        private void btnCauDau_Click(object sender, EventArgs e)
        {
            lvDSCauHoi.SelectedIndex = 0;
        }

        private void btnCauCuoi_Click(object sender, EventArgs e)
        {
            lvDSCauHoi.SelectedIndex = DeThiVuaChon.SoLuongCauHoi - 1;
        }

        private void btnCauSau_Click(object sender, EventArgs e)
        {
            if (Index < DeThiVuaChon.SoLuongCauHoi - 1)
            {
                lvDSCauHoi.SelectedIndex = Index + 1;
            }
        }

        private void btnCauTruoc_Click(object sender, EventArgs e)
        {
            if (Index > 0)
            {
                lvDSCauHoi.SelectedIndex = Index - 1;
            }
        }

        private void LuuDapAnHienTai()
        {
            try
            {
                // Tạm thời tắt sự kiện để tránh stack overflow
                lvDSCauHoi.SelectedIndexChanged -= lvDSCauHoi_SelectedIndexChanged;

                // Lưu đáp án câu hỏi hiện tại
                if (Index >= 0 && Index < dtDapAnDaChon.Rows.Count)
                {
                    dtDapAnDaChon.Rows[Index]["DapAnDaChon"] = LayNoiDungDapAnChon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Bật lại sự kiện
                lvDSCauHoi.SelectedIndexChanged += lvDSCauHoi_SelectedIndexChanged;
            }
        }

        private void LuuLaiDeThi()
        {
            string MaDeThi = DeThiVuaChon.MaDeThi;
            List<CAU_HOI> list = new List<CAU_HOI>();
            foreach (DataRow row in dtDapAnDaChon.Rows)
            {
                string MaCauHoi = row["MaCauHoi"].ToString();
                CAU_HOI ch = new CAU_HOI();
                ch = cauHoiServices.LayThongTinCauHoiTheoMa(MaCauHoi);
                list.Add(ch);
            }
            cTDeThiServices.LuuChiTietDeThi(MaDeThi, list);
        }

        private async Task LayGioNopBai()
        {
            GioKetThuc = await LayNgayGio();
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Bạn chắc chắn muốn nộp bài", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    LuuDapAnHienTai();
                    LuuLaiDeThi();
                    await LayGioNopBai();

                    float Diem = 0;
                    float DiemMoiCau = (float)(10.0 / (DeThiVuaChon.SoLuongCauHoi * 1.0));
                    float SoCauDung = 0;
                    foreach (DataRow row in dtDapAnDaChon.Rows)
                    {
                        string DapAnDung = row["DapAnDung"].ToString().Trim();
                        string DapAnSinhVienChon = row["DapAnDaChon"].ToString().Trim();
                        if (DapAnSinhVienChon.Equals(DapAnDung))
                        {
                            SoCauDung++;
                        }
                    }
                    Diem = (float)Math.Round(SoCauDung * DiemMoiCau, 1);
                    string tenmonthi = lbTenMonThi.Text;
                    //MessageBox.Show($"Bạn được {Diem}\nĐiểm mỗi câu: {DiemMoiCau}");
                    string MaDeThi = DeThiVuaChon.MaDeThi;
                    string MaSV = SVMoiDangNhap.MaSinhVien;
                    //chuyen kieu datetime2 lay tu API sang DateTime truoc khi luu vao CSDL
                    diemThiServices.LuuDiem(MaDeThi, MaSV, int.Parse(SoCauDung.ToString()), Diem, GioVaoThi, GioKetThuc);
                    this.Close();
                    KetQuaThi frm = new KetQuaThi(SVMoiDangNhap, DeThiVuaChon, tenmonthi, Diem, SoCauDung);
                    frm.Show();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
