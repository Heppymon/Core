namespace MyBotCore.Shared.Settings
{
    public class QuartzSettings
    {

        // https://www.freeformatter.com/cron-expression-generator-quartz.html - Cron time generaton
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        public int CoolDown { get; set; }
    }
}
