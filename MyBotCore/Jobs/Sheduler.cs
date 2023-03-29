using Quartz;

namespace MyBotCore.Jobs
{
    [DisallowConcurrentExecution]
    public class Sheduler : IJob
    {
        private readonly ILogger<Sheduler> logger;

        public Sheduler(ILogger<Sheduler> logger)
        {
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // await RunSomething();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }
}
