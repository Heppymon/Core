using MyBotCore.Shared.Enums;
using MyBotCore.Shared.Exceptions;
using MyBotCore.Shared.Responses;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace MyBotCore.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
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
                ExceptionLog(e);

                await UnknownErrorResponseWriteAsync(HttpStatusCode.InternalServerError);
            }

            async Task UnknownErrorResponseWriteAsync(HttpStatusCode httpStatusCode)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)httpStatusCode;

                var json = JsonSerializer.Serialize(new ApiResponseModel(errorCodeEnum: ApiErrorCode.UnKnownError));

                await context.Response.WriteAsync(json);
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
            }

            void ExceptionLog<TException>(TException ex) where TException : Exception
            {
                logger.LogCritical(JsonSerializer.Serialize(new
                {
                    ex.Message,
                    ex.StackTrace,
                    CreateDate = DateTime.UtcNow.ToString("G", CultureInfo.CreateSpecificCulture("en-US"))
                }));
            }
        }
    }
}
