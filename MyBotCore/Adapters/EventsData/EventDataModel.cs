namespace MyBotCore.Adapters.EventsData
{
    public class EventDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Excerpt { get; set; }
        public string Post { get; set; }
        public string Price { get; set; }
        public string Place { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public DateTime Start_At { get; set; } = DateTime.UtcNow;
        public DateTime End_At { get; set; } = DateTime.UtcNow;
        public string Link { get; set; }
    }
}
