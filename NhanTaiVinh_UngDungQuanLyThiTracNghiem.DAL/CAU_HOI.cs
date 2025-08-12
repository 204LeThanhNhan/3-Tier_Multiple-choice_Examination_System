namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CAU_HOI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CAU_HOI()
        {
            CT_DE_THI = new HashSet<CT_DE_THI>();
            DAP_AN = new HashSet<DAP_AN>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string MaCauHoi { get; set; }

        [Required]
        [StringLength(255)]
        public string NoiDung { get; set; }

        [Required]
        [StringLength(10)]
        public string DoKho { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaMon { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string MaGiangVien { get; set; }

        public virtual GIANG_VIEN GIANG_VIEN { get; set; }

        public virtual MON_HOC MON_HOC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_DE_THI> CT_DE_THI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DAP_AN> DAP_AN { get; set; }
    }
}
