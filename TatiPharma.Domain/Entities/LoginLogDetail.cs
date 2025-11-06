using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Domain.Entities;

namespace TatiPharma.Domain.Entities
{
    [Table("tblLoginLogDetails")]
    public class LoginLogDetail
    {
        [Key, Column("LoginLogId")]
        public long LoginLogId { get; set; }

        [Column("bintUserId")]
        public long? UserId { get; set; }

        [Column("DeviceName"), StringLength(50)]
        public string? DeviceName { get; set; }

        [Column("bitIsMobile")]
        public bool? IsMobile { get; set; }

        [Column("LogDate")]
        public DateTime? LogDate { get; set; }

        [Column("LogDesc"), MaxLength(500)]
        public string? LogDesc { get; set; }

        [Column("bitIsActive")]
        public bool? IsActive { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("LogType")]
        public short? LogType { get; set; }

        // Navigation: User (optional)
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
