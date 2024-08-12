using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("UserPasswords", Schema = "dbo")]
    public class UserPassword : BaseEntity
    {
        public string Password { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
