using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblPaymentRMethod")]
    public class PaymentMethod
    {
        [Key, Column("paymentMethodId")]
        public long PaymentMethodId { get; set; }

        [Column("paymentMethod"), StringLength(500)]
        public string? PaymentMethodName { get; set; }

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")]
        public bool? IsDeleted { get; set; }

        // One-to-many
        public ICollection<PaymentRecord> PaymentRecords { get; set; } = new List<PaymentRecord>();
    }
}
