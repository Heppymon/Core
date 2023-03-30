namespace MyBotDb.Models
{
    public class DbUser : DbEntity
    {
        public long ChatId { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public bool SilentMode { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
