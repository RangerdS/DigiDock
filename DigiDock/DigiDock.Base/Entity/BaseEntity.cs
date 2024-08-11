namespace DigiDock.Base.Entity
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public long CreateUserId { get; set; } // fill here: bunlar User user olarak mı tutulmalı?
        public long UpdateUserId { get; set; }
        public long? DeleteUserId { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
