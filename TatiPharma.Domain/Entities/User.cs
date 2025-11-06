using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



    namespace TatiPharma.Domain.Entities
    {
        [Table("Users")]
        public class User
        {
            [Key]
            [Column("bintUserId")]
            public long BintUserId { get; set; }

            [Column("UserName")]
            public string? UserName { get; set; }  // ← ? = nullable

            [Column("Password")]
            public string? Password { get; set; }

            [Column("EmailAddress")]
            public string? EmailAddress { get; set; }

            [Column("strPinCode")]
            public string? StrPinCode { get; set; }

            [Column("strDescription")]
            public string? StrDescription { get; set; }  // nvarchar(MAX) → can be NULL

            [Column("strCity")]
            public string? StrCity { get; set; }

            [Column("strState")]
            public string? StrState { get; set; }

            [Column("phoneNumber")]
            public string? PhoneNumber { get; set; }

            [Column("strFirstName")]
            public string? StrFirstName { get; set; }

            [Column("gender")]
            public string? Gender { get; set; }

            [Column("strMobileNumber")]
            public string? StrMobileNumber { get; set; }

            [Column("strLastName")]
            public string? StrLastName { get; set; }

            [Column("bitIsActive")]
            public bool BitIsActive { get; set; }

            [Column("CreatedDate")]
            public DateTime? CreatedDate { get; set; }

            [Column("isMain")]
            public bool? IsMain { get; set; }

            [Column("dateOfBirth")]
            public DateTime? DateOfBirth { get; set; }

            [Column("bintRollId")]
            public long BintRollId { get; set; }

            [ForeignKey("BintRollId")]
            public UserRole? Role { get; set; }
        }
    }

