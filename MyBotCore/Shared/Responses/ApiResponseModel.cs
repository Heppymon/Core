using MyBotCore.Shared.Enums;
using System.Text.Json.Serialization;

namespace MyBotCore.Shared.Responses
{
    public class ApiResponseModel
    {
        [JsonPropertyName("errorCodeEnum")]
        public ApiErrorCode ErrorCodeEnum { get; set; } = ApiErrorCode.OK;

        [JsonPropertyName("isOperationSucceed")]
        public bool IsOperationSucceed { get; set; } = true;

        [JsonPropertyName("data")]
        public dynamic Data { get; set; }

        public ApiResponseModel(Dictionary<string, object> valuePairs = null, ApiErrorCode? errorCodeEnum = null)
        {
            FillError(errorCodeEnum);

            Data = valuePairs;
        }

        public ApiResponseModel(dynamic valuePairs, ApiErrorCode? errorCodeEnum = null)
        {
            FillError(errorCodeEnum);

            Data = valuePairs;
        }

        private void FillError(ApiErrorCode? errorCodeEnum = null)
        {
            if (errorCodeEnum.HasValue && errorCodeEnum != ApiErrorCode.OK)
            {
                ErrorCodeEnum = errorCodeEnum.GetValueOrDefault();
                IsOperationSucceed = false;
            }
        }
    }
}
