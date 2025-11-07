using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblDrugTypeMaster")]
    public class DrugTypeMaster
    {
        [Key]
        [Column("drugTypeId")]
        public long DrugTypeId { get; set; }

        [Column("drugTypeName")] 
        public string? DrugTypeName { get; set; }

        [Column("bitIsActive")] 
        public bool? BitIsActive { get; set; }

        [Column("dateCreated")] 
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")] 
        public bool? BitIsDelete { get; set; }


        // One-to-many with DrugMaster
        public ICollection<DrugMaster> DrugMasters { get; set; } = new List<DrugMaster>();
    }
}
