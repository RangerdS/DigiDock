using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("UserLogins", Schema = "dbo")]
    public class UserLogin : BaseEntity
    {
        public string IpAddress { get; set; }
        public bool IsLoginSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
