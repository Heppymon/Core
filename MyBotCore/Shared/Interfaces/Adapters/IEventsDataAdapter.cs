using MyBotCore.Adapters.EventsData;

namespace MyBotCore.Shared.Interfaces.Adapters
{
    public interface IEventsDataAdapter
    {
        public Task<List<EventDataModel>> GetEvents();
    }
}
