namespace MyBotCore.Shared.Settings
{
    /// <summary>
    /// Settings for openAi API integration
    /// </summary>
    public class OpenAiSettings
    {
        /// <summary>
        /// i.d.k. if this parameter is needed, but so far it is here
        /// TODO: Check this
        /// </summary>
        public string ApiLogin { get; set; }
        
        /// <summary>
        /// Token for api integration
        /// </summary>
        public string ApiToken { get; set; }
    }
}
