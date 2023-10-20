using MyBotCore.Adapters.EventsData;

namespace MyBotCore.Shared.Interfaces.Services
{
    public interface IEventDataService
    {
        public Task<bool> ActualizeEventsData(List<EventDataModel> eventsData);
    }
}
