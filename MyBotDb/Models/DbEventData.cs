namespace MyBotDb.Models
{
    public class DbEventData : DbEntity
    {
        public int OriginalId { get; set; }
        public string Name { get; set; }
        public string Excerpt { get; set; }
        public string Post { get; set; }
        public string Price { get; set; }
        public string Place { get; set; }
        public string Address { get; set; }
        public DateTime StartAt { get; set; } = DateTime.UtcNow;
        public DateTime EndAt { get; set; } = DateTime.UtcNow;
        public string Link { get; set; }
    }
}
