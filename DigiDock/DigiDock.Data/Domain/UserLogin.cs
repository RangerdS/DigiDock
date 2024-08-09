using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
