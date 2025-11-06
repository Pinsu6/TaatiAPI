using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblCustomerTypeMaster")]
    public class CustomerType
    {
        [Key]
        [Column("cusTypeId")]
        public long CusTypeId { get; set; }  // bigint, NOT NULL

        [Column("cusTypeName")]
        public string? CusTypeName { get; set; }  // nvarchar(500), NULLABLE

        [Column("bitIsActive")]
        public bool? BitIsActive { get; set; }  // bit, NULLABLE

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }  // datetime, NULLABLE

        [Column("bitIsDelete")]
        public bool? BitIsDelete { get; set; }  // bit, NULLABLE
    }
}
