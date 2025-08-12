namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CT_DE_THI
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string MaDeThi { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaMon { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string MaCauHoi { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string MaGiangVien { get; set; }

        public virtual CAU_HOI CAU_HOI { get; set; }

        public virtual DE_THI DE_THI { get; set; }

        //public virtual MON_HOC MON_HOC { get; set; }


        //
        public virtual GIANG_VIEN GIANG_VIEN { get; set; }
    }
}
