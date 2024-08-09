using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
