namespace QuanLyNhaHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietHoaDon")]
    public partial class ChiTietHoaDon
    {
        [Key]
        public int CTHD_ID { get; set; }

        public int? HoaDonID { get; set; }

        public int? MonID { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public virtual HoaDon HoaDon { get; set; }

        public virtual ThucDon ThucDon { get; set; }
    }
}
