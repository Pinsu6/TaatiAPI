using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
   
    [Table("tblCustomer")]  // Adjust if table name differs
    public class Customer
    {
        [Key]
        [Column("CustomerId")]
        public long CustomerId { get; set; }  // bigint, not nullable (Unchecked)

        [Column("cusCode")]
        public string? CusCode { get; set; }  // nvarchar(500), nullable (Checked)

        [Column("cusFirstname")]
        public string? CusFirstname { get; set; }  // nvarchar(500), nullable

        [Column("cusLastname")]
        public string? CusLastname { get; set; }  // nvarchar(500), nullable

        [Column("cusMobileno")]
        public string? CusMobileno { get; set; }  // nvarchar(500), nullable

        [Column("cusPhonenoO")]
        public string? CusPhonenoO { get; set; }  // nvarchar(500), nullable

        [Column("cusPhonenoR")]
        public string? CusPhonenoR { get; set; }  // nvarchar(500), nullable

        [Column("cusEmail")]
        public string? CusEmail { get; set; }  // nvarchar(500), nullable

        [Column("city")]
        public string? City { get; set; }  // nvarchar(500), nullable

        [Column("address")]
        public string? Address { get; set; }  // nvarchar(500), nullable

        [Column("pin")]
        public string? Pin { get; set; }  // nvarchar(500), nullable

        [Column("district")]
        public string? District { get; set; }  // nvarchar(500), nullable

        [Column("country")]
        public string? Country { get; set; }  // nvarchar(500), nullable

        [Column("employeeId")]
        public long? EmployeeId { get; set; }  // bigint, nullable

        [Column("bitIsActive")]
        public bool? BitIsActive { get; set; }  // bit, nullable

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }  // date, nullable

        [Column("bitIsDelete")]
        public bool? BitIsDelete { get; set; }  // bit, nullable

        [Column("storeAmtremain")]
        public decimal? StoreAmtremain { get; set; }  // decimal(18,2), nullable

        [Column("storeAmtused")]
        public decimal? StoreAmtused { get; set; }  // decimal(18,2), nullable

        [Column("Region")]
        public string? Region { get; set; }  // nvarchar(200), nullable

        [Column("pbsllicense")]
        public string? Pbsllicense { get; set; }  // nvarchar(200), nullable

        [Column("LicenseType")]
        public string? LicenseType { get; set; }  // nvarchar(200), nullable

        [Column("LicenseExpiry")]
        public DateTime? LicenseExpiry { get; set; }  // date, nullable

        [Column("cusTypeId")]
        public long? CusTypeId { get; set; }  // bigint, nullable

        [Column("creditlim")]
        public decimal? Creditlim { get; set; }  // decimal(18,0), nullable

        [Column("creditdays")]
        public int? Creditdays { get; set; }  // int, nullable

        [ForeignKey("CusTypeId")]
        public CustomerType? CustomerType { get; set; }  // Navigation to CustomerType

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }  // Navigation to Employee
    }
}
