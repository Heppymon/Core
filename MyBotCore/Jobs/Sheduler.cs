using MyBotCore.Shared.Interfaces.Adapters;
using MyBotCore.Shared.Interfaces.Services;
using Quartz;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics;

namespace MyBotCore.Jobs
{
    [DisallowConcurrentExecution]
    public class Sheduler : IJob
    {
        private readonly IEventsDataAdapter dataAdapter;
        private readonly IEventDataService eventDataService;

        public Sheduler(IEventsDataAdapter eventDataAdapter, IEventDataService eventDataService)
        {
            this.dataAdapter = eventDataAdapter;
            this.eventDataService = eventDataService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var actualizeResult = await eventDataService.ActualizeEventsData(await dataAdapter.GetEvents());
                Log.Information(actualizeResult ? "Events data has been updated successfully" : "Events data update failed");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
