namespace NhanTaiVinh_UngDungQuanLyThiTracNghiem.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.SqlClient;

    public partial class DE_THI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DE_THI()
        {
            CT_DE_THI = new HashSet<CT_DE_THI>();
            DIEMs = new HashSet<DIEM>();
        }

        [Key]
        [StringLength(50)]
        public string MaDeThi { get; set; }

        [Required]
        [StringLength(50)]
        public string MaCaThi { get; set; }

        public int ThoiGianLamBai { get; set; }


        public int SoLuongCauHoi { get; set; }

        public int SoLuongDe { get; set; }

        public int SoLuongVua { get; set; }

        public int SoLuongKho { get; set; }

        [Column(TypeName = "bit")]
        public bool SuDung { get; set; }
        public virtual CA_THI CA_THI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_DE_THI> CT_DE_THI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIEM> DIEMs { get; set; }


        public DataTable RandomCauHoiChoDeThi(string maMon, int slDe, int slVua, int slKho)
        {
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            using (SqlCommand cmd = new SqlCommand("RamdomQuestion", db.Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm tham số đầu vào
                cmd.Parameters.AddWithValue("@SoCau", slDe + slVua + slKho);
                cmd.Parameters.AddWithValue("@MaMon", maMon);
                cmd.Parameters.AddWithValue("@SLDe", slDe);
                cmd.Parameters.AddWithValue("@SLVua", slVua);
                cmd.Parameters.AddWithValue("@SLKho", slKho);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                return dt;
            }
        }

        public CAU_HOI_IN_EXAM LayMotCauHoiTrongDeThi(string MaCauHoi)
        {
            CAU_HOI_IN_EXAM cauHoi = null;
            ThiTracNghiemDB db = new ThiTracNghiemDB();
            using (SqlCommand cmd = new SqlCommand("LayThongTinCauHoiTheoMa", db.Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ma", MaCauHoi);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cauHoi = new CAU_HOI_IN_EXAM()
                        {
                            MaCauHoi = reader["MaCauHoi"].ToString(),
                            NoiDung = reader["NoiDung"].ToString(),
                            DapAnA = reader["DapAnA"].ToString(),
                            DapAnB = reader["DapAnB"].ToString(),
                            DapAnC = reader["DapAnC"].ToString(),
                            DapAnD = reader["DapAnD"].ToString(),
                            DapAnDung = reader["DapAnDung"].ToString()
                        };
                        reader.Close();
                        return cauHoi;
                    }
                }
            }
            return cauHoi;
        }
    }
}
