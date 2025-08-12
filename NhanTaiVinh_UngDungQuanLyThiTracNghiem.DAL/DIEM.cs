namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DIEM")]
    public partial class DIEM
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string MaDeThi { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaSinhVien { get; set; }

        public int SoCauDung { get; set; }

        public double DiemThi { get; set; }

        public DateTime ThoiGianVaoLam { get; set; }

        public DateTime ThoiGianNop { get; set; }

        public virtual DE_THI DE_THI { get; set; }

        public virtual SINH_VIEN SINH_VIEN { get; set; }
    }
}
