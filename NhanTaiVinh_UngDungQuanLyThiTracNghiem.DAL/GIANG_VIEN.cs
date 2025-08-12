namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class GIANG_VIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GIANG_VIEN()
        {
            CAU_HOI = new HashSet<CAU_HOI>();
        }

        [Key]
        [StringLength(50)]
        public string MaGiangVien { get; set; }

        [Required]
        [StringLength(255)]
        public string TenGiangVien { get; set; }

        [Required]
        [StringLength(15)]
        public string SDT { get; set; }

        [Required]
        [StringLength(255)]
        public string DiaChi { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string UsernameGV { get; set; }

        [Required]
        [StringLength(20)]
        public string PasswordGV { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAU_HOI> CAU_HOI { get; set; }
    }
}
