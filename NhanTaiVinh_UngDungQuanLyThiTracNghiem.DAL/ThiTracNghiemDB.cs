using System.Data.Entity;
using System.Data.SqlClient;

namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    public partial class ThiTracNghiemDB : DbContext
    {
        public SqlConnection Connection { get; private set; }
        public ThiTracNghiemDB()
            : base("name=ThiTracNghiemDB")
        {
            string connectionString = "data source=LAPTOP-QRM68HB0\\SQL2019EXPRESS;initial catalog=QLThiTracNghiem;integrated security=True;encrypt=False;MultipleActiveResultSets=True;App=EntityFramework";

            // Tạo kết nối đến cơ sở dữ liệu
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public virtual DbSet<CA_THI> CA_THI { get; set; }
        public virtual DbSet<CAU_HOI> CAU_HOI { get; set; }
        public virtual DbSet<CT_DE_THI> CT_DE_THI { get; set; }
        public virtual DbSet<DAP_AN> DAP_AN { get; set; }
        public virtual DbSet<DE_THI> DE_THI { get; set; }
        public virtual DbSet<DIEM> DIEMs { get; set; }
        public virtual DbSet<GIANG_VIEN> GIANG_VIEN { get; set; }
        public virtual DbSet<MON_HOC> MON_HOC { get; set; }
        public virtual DbSet<QUAN_LY_NHAN_SU> QUAN_LY_NHAN_SU { get; set; }
        public virtual DbSet<SINH_VIEN> SINH_VIEN { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CA_THI>()
                .HasMany(e => e.DE_THI)
                .WithRequired(e => e.CA_THI)
                .WillCascadeOnDelete(false);

            // Thiết lập mối quan hệ giữa CA_THI và MON_HOC
            modelBuilder.Entity<CA_THI>()
                .HasRequired(ca => ca.MON_HOC) // CA_THI bắt buộc có MON_HOC
                .WithMany(mon => mon.CA_THI)   // Một MON_HOC có nhiều CA_THI
                .HasForeignKey(ca => ca.MaMon) // Khóa ngoại trong CA_THI là MaMon
                .WillCascadeOnDelete(false);   // Không xóa cascade khi xóa MON_HOC

            modelBuilder.Entity<CAU_HOI>()
                .HasMany(e => e.CT_DE_THI)
                .WithRequired(e => e.CAU_HOI)
                .HasForeignKey(e => new { e.MaMon, e.MaCauHoi, e.MaGiangVien })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CAU_HOI>()
                .HasMany(e => e.DAP_AN)
                .WithRequired(e => e.CAU_HOI)
                .HasForeignKey(e => new { e.MaCauHoi, e.MaMon, e.MaGiangVien })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DE_THI>()
                .HasMany(e => e.CT_DE_THI)
                .WithRequired(e => e.DE_THI)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DE_THI>()
                .HasMany(e => e.DIEMs)
                .WithRequired(e => e.DE_THI)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GIANG_VIEN>()
                .HasMany(e => e.CAU_HOI)
                .WithRequired(e => e.GIANG_VIEN)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MON_HOC>()
                .HasMany(e => e.CAU_HOI)
                .WithRequired(e => e.MON_HOC)
                .WillCascadeOnDelete(false);

            /*modelBuilder.Entity<MON_HOC>()
                .HasMany(e => e.CT_DE_THI)
                .WithRequired(e => e.MON_HOC)
                .WillCascadeOnDelete(false);*/

            modelBuilder.Entity<SINH_VIEN>()
                .HasMany(e => e.DIEMs)
                .WithRequired(e => e.SINH_VIEN)
                .WillCascadeOnDelete(false);
        }
    }
}
