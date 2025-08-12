namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MON_HOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MON_HOC()
        {
            CAU_HOI = new HashSet<CAU_HOI>();
            //CT_DE_THI = new HashSet<CT_DE_THI>();
            CA_THI = new HashSet<CA_THI>();  // Thêm bộ sưu tập CA_THI
        }

        [Key]
        [StringLength(50)]
        public string MaMon { get; set; }

        [Required]
        [StringLength(255)]
        public string TenMon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAU_HOI> CAU_HOI { get; set; }

        // Liên kết với CA_THI
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CA_THI> CA_THI { get; set; }  // Thêm thuộc tính CA_THI

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CT_DE_THI> CT_DE_THI { get; set; }
    }
}
