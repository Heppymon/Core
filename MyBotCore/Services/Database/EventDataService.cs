using MyBotCore.Adapters.EventsData;
using MyBotCore.Shared.Interfaces.Services;
using MyBotDb.Models;

namespace MyBotCore.Services.Database
{
    public class EventDataService : IEventDataService
    {
        private readonly IRepo repo;

        public EventDataService(IRepo repo)
        {
            this.repo = repo;
        }

        public async Task<bool> ActualizeEventsData(List<EventDataModel> eventsData)
        {
            var eventsInDb = repo.Get<DbEventData>();

            foreach (var eventData in eventsInDb)
                if (!eventsData.Any(x => x.Id == eventData.OriginalId))
                    await repo.DeleteAsync(eventData);

            foreach (var eventData in eventsData)
                if (!eventsInDb.Any(x => x.OriginalId == eventData.Id))
                    await repo.AddAsync(new DbEventData()
                    {
                        OriginalId = eventData.Id,
                        Name = eventData.Name,
                        Post = eventData.Post,
                        Address = eventData.Address,
                        StartAt = eventData.Start_At,
                        EndAt = eventData.End_At,
                        Excerpt = eventData.Excerpt,
                        Link = eventData.Link,
                        Place = eventData.Place,
                        Price = eventData.Price
                    });

            return true;
        }
    }
}
