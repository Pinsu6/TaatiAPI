using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblPrinterMaster")]
    public class PrinterMaster
    {
        [Key, Column("printerId")]
        public long PrinterId { get; set; }

        [Column("reportName"), MaxLength(50)]
        public string? ReportName { get; set; }

        [Column("printerName"), MaxLength(50)]
        public string? PrinterName { get; set; }

        [Column("paperSize"), MaxLength(50)]
        public string? PaperSize { get; set; }

        [Column("costSaveprint"), MaxLength(1)]
        public string? CostSavePrint { get; set; }

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("bitIsDelete")]
        public bool? IsDeleted { get; set; }
    }
}
