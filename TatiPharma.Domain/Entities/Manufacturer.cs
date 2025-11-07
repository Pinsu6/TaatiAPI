using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblManufacturer")]
    public class Manufacturer
    {
        [Key]
        [Column("manufacturerId")]
        public long ManufacturerId { get; set; }

        [Column("manufacturerName")] 
        public string? ManufacturerName { get; set; }

        [Column("mfg")] 
        public string? Mfg { get; set; }

        [Column("mfgPerson")] 
        public string? MfgPerson { get; set; }

        [Column("mfgPosition")] 
        public string? MfgPosition { get; set; }

        [Column("mfgEmail")] 
        public string? MfgEmail { get; set; }

        [Column("mfgMobile")] 
        public string? MfgMobile { get; set; }

        [Column("mfgAddress")] 
        public string? MfgAddress { get; set; }

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
