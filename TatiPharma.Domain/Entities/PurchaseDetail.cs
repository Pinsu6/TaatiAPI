using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblPurchaseDetail")]
    public class PurchaseDetail
    {
        [Key, Column("PurchaseDtlid")]
        public long PurchaseDetailId { get; set; }

        [Column("Purchaseid")]
        public long? PurchaseId { get; set; }

        [Column("drugId")]
        public long? DrugId { get; set; }

        [Column("UnitPack"), StringLength(500)]
        public string? UnitPack { get; set; }

        [Column("UnitQty")]
        public long? UnitQty { get; set; }

        [Column("BatchNo"), StringLength(100)]
        public string? BatchNo { get; set; }

        [Column("ExpireDate")]
        public DateTime? ExpireDate { get; set; }

        [Column("BarcodeNo"), StringLength(100)]
        public string? BarcodeNo { get; set; }

        [Column("UnitCost")]
        public decimal? UnitCost { get; set; }

        [Column("taxDetailsId")]
        public long? TaxDetailsId { get; set; }

        [Column("DQty")]
        public long? DQty { get; set; }

        [Column("DCost")]
        public decimal? DCost { get; set; }

        [Column("Margin")]
        public decimal? Margin { get; set; }

        [Column("SalePrice")]
        public decimal? SalePrice { get; set; }

        [Column("Amount")]
        public decimal? Amount { get; set; }

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("bitIsSave")]
        public bool? IsSaved { get; set; }

        [Column("remainStock")]
        public long? RemainStock { get; set; }

        [Column("saleStock")]
        public long? SaleStock { get; set; }

        [Column("id")]
        public long? Id { get; set; }

        [Column("purchaseStock")]
        public long? PurchaseStock { get; set; }

        [Column("drugQuickcode"), StringLength(500)]
        public string? DrugQuickcode { get; set; }

        [Column("MarginAmt")]
        public decimal? MarginAmt { get; set; }

        [Column("taxid")]
        public long? TaxId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(PurchaseId))]
        public Purchase? Purchase { get; set; }

        [ForeignKey(nameof(DrugId))]
        public DrugMaster? Drug { get; set; }
    }
}
