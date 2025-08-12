using System;
using System.Windows.Forms;
using NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            /*SINH_VIEN s = new SINH_VIEN()
            {
                MaSinhVien = "LNCH12162024",
                HoTen = "Lê Nguyễn Chăm Học",
                NgaySinh = DateTime.Parse("11/10/2004"),
                QueQuan = "Hồ Chí Minh",
                Lop = "22DTHA6"
            };
            Application.Run(new ThucHienThi(s));*/

        }
    }
}
