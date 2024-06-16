using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using NewsletterSubscriberPublisher.Exceptions;
using System.Net;

namespace NewsletterSubscriberPublisher.Middlewares;

internal class ExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch(Exception ex)
        {
            var logger = context.GetLogger<ExceptionHandlerMiddleware>();

            var request = await context.GetHttpRequestDataAsync();
            var response = request.CreateResponse();

            switch (ex)
            {
                case InvalidEmailException:
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await response.WriteStringAsync(ex.Message);
                    break;
                default:
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    await response.WriteStringAsync("Internal server error!");
                    break;
            }

            logger.LogError(ex, ex.Message);
        }
    }
}
