namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System.ComponentModel.DataAnnotations;

    public partial class QUAN_LY_NHAN_SU
    {
        [Key]
        [StringLength(50)]
        public string MaAdmin { get; set; }

        [Required]
        [StringLength(255)]
        public string TenAdmin { get; set; }

        [Required]
        [StringLength(20)]
        public string UsernameADM { get; set; }

        [Required]
        [StringLength(20)]
        public string PasswordADM { get; set; }
    }
}
