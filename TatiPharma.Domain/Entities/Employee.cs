using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Domain.Entities
{
    [Table("tblEmployee")]
    public class Employee
    {
        [Key]
        [Column("employeeId")]
        public long EmployeeId { get; set; }  // bigint, NOT NULL

        [Column("employeeName")]
        public string EmployeeName { get; set; } = string.Empty;  // nvarchar(500), NOT NULL

        [Column("empShort")]
        public string EmpShort { get; set; } = string.Empty;  // nvarchar(500), NOT NULL

        [Column("empPerson")]
        public string? EmpPerson { get; set; }  // nvarchar(500), NULLABLE

        [Column("empPosition")]
        public string? EmpPosition { get; set; }  // nvarchar(500), NULLABLE

        [Column("empEmail")]
        public string? EmpEmail { get; set; }  // nvarchar(500), NULLABLE

        [Column("empMobile")]
        public string? EmpMobile { get; set; }  // nvarchar(500), NULLABLE

        [Column("empAddress")]
        public string? EmpAddress { get; set; }  // nvarchar(500), NULLABLE

        [Column("empType")]
        public string? EmpType { get; set; }  // nvarchar(500), NULLABLE

        [Column("bitIsActive")]
        public bool? BitIsActive { get; set; }  // bit, NULLABLE

        [Column("dateCreated")]
        public DateTime? DateCreated { get; set; }  // date, NULLABLE

        [Column("bitIsDelete")]
        public bool? BitIsDelete { get; set; }  // bit, NULLABLE

        [Column("empPassword")]
        public string? EmpPassword { get; set; }  // nvarchar(500), NULLABLE

        [Column("bintUserId")]
        public long? BintUserId { get; set; }  // bigint, NULLABLE

        [Column("EmpStartDate")]
        public DateTime? EmpStartDate { get; set; }  // date, NULLABLE
    }
}
