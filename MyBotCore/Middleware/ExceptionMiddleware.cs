using MyBotCore.Shared.Enums;
using MyBotCore.Shared.Exceptions;
using MyBotCore.Shared.Responses;
using Serilog;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace MyBotCore.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (BusinessLogicException e)
            {
                await ErrorResponseWriteAsync(HttpStatusCode.BadRequest, e);
            }
            catch (Exception e)
            {
                await UnknownErrorResponseWriteAsync(HttpStatusCode.InternalServerError, e);
            }

            async Task UnknownErrorResponseWriteAsync(HttpStatusCode httpStatusCode, Exception exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)httpStatusCode;

                var json = JsonSerializer.Serialize(new ApiResponseModel(errorCodeEnum: ApiErrorCode.UnKnownError));

                await context.Response.WriteAsync(json);
                Log.Error(JsonSerializer.Serialize(new
                {
                    exception.Message,
                    exception.StackTrace,
                    CreateDate = DateTime.UtcNow.ToString("G", CultureInfo.CreateSpecificCulture("en-US"))
                }));
            }

            async Task ErrorResponseWriteAsync(HttpStatusCode httpStatusCode, BusinessLogicException exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)httpStatusCode;

                await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new ApiResponseModel(errorCodeEnum: exception.ErrorCode)
                    {
                        Data = exception.Data
                    }));

                Log.Warning(JsonSerializer.Serialize(new
                {
                    exception.Message,
                    exception.StackTrace,
                    CreateDate = DateTime.UtcNow.ToString("G", CultureInfo.CreateSpecificCulture("en-US"))
                }));
            }
        }
    }
}
