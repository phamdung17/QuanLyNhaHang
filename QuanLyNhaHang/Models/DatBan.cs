namespace QuanLyNhaHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DatBan")]
    public partial class DatBan
    {
        public int DatBanID { get; set; }

        public int? BanID { get; set; }

        public int? UserID { get; set; }

        public DateTime? NgayDat { get; set; }

        

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; }

        public virtual BanAn BanAn { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
