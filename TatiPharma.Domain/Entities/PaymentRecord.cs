using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblPaymentRecord")]
    public class PaymentRecord
    {
        [Key, Column("paymentId")]
        public long PaymentId { get; set; }

        [Column("SalesInvoiceid")]
        public long? SalesInvoiceId { get; set; }

        [Column("paymentdate")]
        public DateTime? PaymentDate { get; set; }

        [Column("paymentMethodId")]
        public long? PaymentMethodId { get; set; }

        [Column("paymentaccount"), StringLength(500)]
        public string? PaymentAccount { get; set; }

        [Column("PaymentAmount")]
        public decimal? PaymentAmount { get; set; }

        [Column("notes")]
        public string? Notes { get; set; } // nvarchar(MAX)

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")]
        public bool? IsDeleted { get; set; }

        [Column("CustomerId")]
        public long? CustomerId { get; set; }

        // Navigations
        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod? PaymentMethod { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        [ForeignKey(nameof(SalesInvoiceId))]
        public SalesInvoice? SalesInvoice { get; set; }
    }
}
