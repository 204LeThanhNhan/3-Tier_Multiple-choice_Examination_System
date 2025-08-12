namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System.ComponentModel.DataAnnotations;

    public partial class DAP_AN
    {
        [Key]
        [StringLength(50)]
        public string MaDapAN { get; set; }

        [Required]
        [StringLength(50)]
        public string MaCauHoi { get; set; }

        [Required]
        [StringLength(50)]
        public string MaGiangVien { get; set; }

        [Required]
        [StringLength(50)]
        public string MaMon { get; set; }

        [Required]
        [StringLength(255)]
        public string NoiDungDapAn { get; set; }

        public bool DungSai { get; set; }

        public virtual CAU_HOI CAU_HOI { get; set; }
    }
}
