namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CA_THI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CA_THI()
        {
            DE_THI = new HashSet<DE_THI>();
        }

        [Key]
        [StringLength(50)]
        public string MaCaThi { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayCaThi { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan GioBatDau { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan GioKetThuc { get; set; }

        [StringLength(50)]
        public string MaMon { get; set; }  // Thêm cột MaMon

        // Liên kết với bảng MON_HOC
        public virtual MON_HOC MON_HOC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DE_THI> DE_THI { get; set; }
    }
}
