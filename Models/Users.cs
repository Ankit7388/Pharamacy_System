using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string UserName { get; set; }
        [Column(TypeName = "varchar(100)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string UserEmail { get; set; }
        public string RoleName { get; private set; } = "user";

    }
    public class Jwt
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public string Subject { get; set; }
    }
}
