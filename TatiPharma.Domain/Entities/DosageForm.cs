using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblDosageForm")]
    public class DosageForm
    {
        [Key]
        [Column("dosageId")]
        public long DosageId { get; set; }

        [Column("dosageFormname")] 
        public string? DosageFormname { get; set; }

        [Column("dosageType")] 
        public string? DosageType { get; set; }

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
