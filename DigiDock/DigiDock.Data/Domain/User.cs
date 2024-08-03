using DigiDock.Base.Entity;

namespace DigiDock.Data.Domain
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal PointsBalance { get; set; }
    }
}
