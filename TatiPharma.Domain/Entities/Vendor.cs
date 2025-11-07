using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblVender")]
    public class Vendor
    {
        [Key, Column("Venderid")]
        public long VendorId { get; set; }

        [Column("VendorName"), StringLength(50)]
        public string? VendorName { get; set; }

        [Column("CreditLimit")]
        public decimal? CreditLimit { get; set; }

        [Column("CreditDays"), StringLength(50)]
        public string? CreditDays { get; set; }

        [Column("MobileNo"), StringLength(50)]
        public string? MobileNo { get; set; }

        [Column("City"), StringLength(100)]
        public string? City { get; set; }

        [Column("Address"), StringLength(500)]
        public string? Address { get; set; }

        [Column("OpeningBalance")]
        public decimal? OpeningBalance { get; set; }

        [Column("BalanceType"), StringLength(50)]
        public string? BalanceType { get; set; }

        [Column("ClosingBalance")]
        public decimal? ClosingBalance { get; set; }

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")]
        public bool? IsDeleted { get; set; }

        [Column("ClosingBalanceVendor")]
        public decimal? ClosingBalanceVendor { get; set; }

        [Column("PaymentTerm"), StringLength(50)]
        public string? PaymentTerm { get; set; }

        // Navigation
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
