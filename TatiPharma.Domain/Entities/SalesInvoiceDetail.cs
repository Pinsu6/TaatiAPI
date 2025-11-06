using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblSalesInvoiceDetail1")]
    public class SalesInvoiceDetail
    {
        [Key, Column("SalesInvoiceDtlid")]
        public long SalesInvoiceDetailId { get; set; }

        [Column("SalesInvoiceid")]
        public long? SalesInvoiceId { get; set; }

        [Column("drugId")]
        public long? DrugId { get; set; }

        [Column("srno")]
        public long? SrNo { get; set; }

        [Column("Qty")]
        public long? Qty { get; set; }

        [Column("SLRate")]
        public decimal? SaleRate { get; set; }

        [Column("Disc")]
        public decimal? Discount { get; set; }

        [Column("TotalAmt")]
        public decimal? TotalAmount { get; set; }

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")]
        public bool? IsDeleted { get; set; }

        [Column("PurchaseDtlid")]
        public long? PurchaseDetailId { get; set; }

        [Column("bitIsSave")]
        public bool? IsSaved { get; set; }

        [Column("Invoice_Type"), StringLength(50)]
        public string? InvoiceType { get; set; }

        [Column("PaymentBy"), StringLength(50)]
        public string? PaymentBy { get; set; }

        [Column("CustomerId")]
        public long? CustomerId { get; set; }

        [Column("id")]
        public long? Id { get; set; }

        [Column("filename")]
        public string? Filename { get; set; }

        [Column("taxDetailsId")]
        public long? TaxDetailsId { get; set; }

        [Column("taxRate")]
        public decimal? TaxRate { get; set; }

        [Column("taxamt")]
        public decimal? TaxAmount { get; set; }

        [Column("Discamt")]
        public decimal? DiscountAmount { get; set; }

        [Column("taxid")]
        public long? TaxId { get; set; }

        // Navigation
        [ForeignKey(nameof(SalesInvoiceId))]
        public SalesInvoice? SalesInvoice { get; set; }

        [ForeignKey(nameof(DrugId))]
        public DrugMaster? Drug { get; set; }

        [ForeignKey(nameof(PurchaseDetailId))]
        public PurchaseDetail? PurchaseDetail { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }
    }
}
