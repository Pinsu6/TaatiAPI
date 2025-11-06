using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblSalesInvoice")]
    public class SalesInvoice
    {
        [Key, Column("SalesInvoiceid")]
        public long SalesInvoiceId { get; set; }

        [Column("cusName"), StringLength(50)]
        public string? CustomerName { get; set; }

        [Column("cusCode")]
        public long? CustomerCode { get; set; }

        [Column("Invoice_Type"), StringLength(50)]
        public string? InvoiceType { get; set; }

        [Column("Order_No"), StringLength(50)]
        public string? OrderNo { get; set; }

        [Column("BillDate")]
        public DateTime? BillDate { get; set; }

        [Column("City"), StringLength(100)]
        public string? City { get; set; }

        [Column("Series"), StringLength(50)]
        public string? Series { get; set; }

        [Column("Invoice_No"), StringLength(50)]
        public string? InvoiceNo { get; set; }

        [Column("Sales_AC")]
        public long? SalesAccount { get; set; }

        [Column("DueDate")]
        public DateTime? DueDate { get; set; }

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

        [Column("CustomerId")]
        public long? CustomerId { get; set; }

        [Column("employee"), StringLength(50)]
        public string? Employee { get; set; }

        [Column("PaymentStatus"), StringLength(50)]
        public string? PaymentStatus { get; set; }

        [Column("PaidAmount")]
        public decimal? PaidAmount { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        // Navigation
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        public ICollection<SalesInvoiceDetail> Details { get; set; } = new List<SalesInvoiceDetail>();
        public ICollection<PaymentRecord> PaymentRecords { get; set; } = new List<PaymentRecord>();
    }
}
