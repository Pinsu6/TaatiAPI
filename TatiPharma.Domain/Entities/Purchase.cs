using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblPurchase")]
    public class Purchase
    {
        [Key, Column("Purchaseid")]
        public long PurchaseId { get; set; }

        [Column("Venderid")]
        public long? VendorId { get; set; }

        [Column("City"), StringLength(100)]
        public string? City { get; set; }

        [Column("curruntAccount")]
        public long? CurrentAccount { get; set; }

        [Column("perchaseAccount")]
        public long? PurchaseAccount { get; set; }

        [Column("invoiceType"), StringLength(50)]
        public string? InvoiceType { get; set; }

        [Column("taxType"), StringLength(100)]
        public string? TaxType { get; set; }

        [Column("BillNo"), StringLength(50)]
        public string? BillNo { get; set; }

        [Column("BillDate")]
        public DateTime? BillDate { get; set; }

        [Column("Division"), StringLength(50)]
        public string? Division { get; set; }

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")]
        public bool? IsDeleted { get; set; }

        [Column("bitisSave")]
        public bool? IsSaved { get; set; }

        [Column("TotalAmount")]
        public decimal? TotalAmount { get; set; }

        [ForeignKey(nameof(VendorId))]
        public Vendor? Vendor { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}
