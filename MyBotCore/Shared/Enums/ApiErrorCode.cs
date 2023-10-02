namespace MyBotCore.Shared.Enums
{
    public enum ApiErrorCode
    {
        OK = 0,
        UnKnownError = 1,

        // TgApi 4xx
        NullUpdateModel = 400,

    }
}
