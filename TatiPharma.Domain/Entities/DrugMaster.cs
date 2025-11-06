using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblDrugMaster")]
    public class DrugMaster
    {
        [Key]
        [Column("drugId")]
        public long DrugId { get; set; }

        // … (all the scalar columns you already have) …
        [Column("drugCode")] 
        public string? DrugCode { get; set; }

        [Column("drugQuickcode")] 
        public string? DrugQuickcode { get; set; }

        [Column("drugName")] 
        public string? DrugName { get; set; }

        [Column("drugShort")] 
        public string? DrugShort { get; set; }

        [Column("theRapeuticclass")] 
        public string? TheRapeuticclass { get; set; }

        [Column("drugNdccode")] 
        public string? DrugNdccode { get; set; }

        [Column("strength")] 
        public string? Strength { get; set; }

        [Column("brandName")] 
        public string? BrandName { get; set; }

        [Column("quantityPack")] 
        public string? QuantityPack { get; set; }

        [Column("maxLevel")] 
        public int? MaxLevel { get; set; }

        [Column("minLevel")] 
        public int? MinLevel { get; set; }

        [Column("narcotics")] 
        public bool? Narcotics { get; set; }

        [Column("bitIsActive")] 
        public bool? BitIsActive { get; set; }

        [Column("dateCreated")] 
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")] 
        public bool? BitIsDelete { get; set; }

        [Column("Margin")] 
        public decimal? Margin { get; set; }

        [Column("UnitCost")] 
        public decimal? UnitCost { get; set; }

        [Column("DrugType")] 
        public string? DrugType { get; set; }


        // ---------- FK columns ----------
        [Column("drugTypeId")] 
        public long? DrugTypeId { get; set; }

        [Column("dosageId")] 
        public long? DosageId { get; set; }

        [Column("udiId")] 
        public long? UdiId { get; set; }          // (you may have a separate table for UDI)

        [Column("uomId")] 
        public long? UomId { get; set; }          // (you may have a separate table for UOM)

        [Column("manufacturerId")] 
        public long? ManufacturerId { get; set; }

        [Column("taxDetailsId")] 
        public long? TaxDetailsId { get; set; }   // (you may have a TaxDetails table)

        [Column("taxid")] 
        public long? Taxid { get; set; }          // (you may have a Tax table)\

        


        // ---------- Navigation properties ----------
        [ForeignKey(nameof(DrugTypeId))]
        public DrugTypeMaster? DrugTypeMaster { get; set; }

        [ForeignKey(nameof(DosageId))]
        public DosageForm? DosageForm { get; set; }

        [ForeignKey(nameof(ManufacturerId))]
        public Manufacturer? Manufacturer { get; set; }

        
    }
}
