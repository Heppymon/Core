using MyBotCore.Shared.Enums;

namespace MyBotCore.Shared.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public ApiErrorCode ErrorCode { get; private set; }
        public dynamic Data { get; private set; }

        public BusinessLogicException(ApiErrorCode errorCode)
        {
            ErrorCode = errorCode;
            Data = errorCode.ToString();
        }

        public BusinessLogicException(ApiErrorCode errorCode, dynamic data)
        {
            ErrorCode = errorCode;
            Data = data;
        }
    }
}
