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
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        [Column("bintRollId")]
        public long BintRollId { get; set; }

        [Column("RollName")]
        public string RollName { get; set; } = string.Empty;

        [Column("Description")]
        public string? Description { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
