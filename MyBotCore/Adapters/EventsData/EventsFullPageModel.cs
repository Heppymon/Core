namespace MyBotCore.Adapters.EventsData
{
    public class EventsFullPageModel
    {
        public int current_page { get; set; }
        public List<EventDataModel> data { get; set; }
        public string first_page_url { get; set; }
        public string from { get; set; }
        public string last_page { get; set; }
        public string last_page_url { get; set; }
        public int total { get; set; }

    }
}
