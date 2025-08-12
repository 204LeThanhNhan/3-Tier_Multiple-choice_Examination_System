namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SINH_VIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SINH_VIEN()
        {
            DIEMs = new HashSet<DIEM>();
        }

        [Key]
        [StringLength(50)]
        public string MaSinhVien { get; set; }

        [Required]
        [StringLength(255)]
        public string HoTen { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgaySinh { get; set; }

        [Required]
        [StringLength(50)]
        public string QueQuan { get; set; }

        [Required]
        [StringLength(50)]
        public string Lop { get; set; }

        [Column(TypeName = "image")]
        public byte[] HinhDaiDien { get; set; }

        [Required]
        [StringLength(20)]
        public string UsernameSV { get; set; }

        [Required]
        [StringLength(20)]
        public string PasswordSV { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIEM> DIEMs { get; set; }
    }
}
