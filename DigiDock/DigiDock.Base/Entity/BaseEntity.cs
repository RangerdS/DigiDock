namespace DigiDock.Base.Entity
{
    public class BaseEntity
    {

        public long Id { get; set; }
        public bool IsActive { get; set; } = true;

        public long CreateUser { get; set; }
        public long UpdateUser { get; set; }
        public long? DeleteUser { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
