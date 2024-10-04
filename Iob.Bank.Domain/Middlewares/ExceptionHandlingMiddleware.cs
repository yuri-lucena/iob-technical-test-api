using System.Net;
using System.Text.Json;
using Iob.Bank.Domain.Data;
using Iob.Bank.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Iob.Bank.Domain.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is NotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(Response<string>.CreateError(
                $"{ex.InnerException?.Message ?? ex.Message}")));
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(Response<string>.CreateError(
                $"{ex.InnerException?.Message ?? ex.Message}")));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(Response<string>.CreateError(
                $"{ex.InnerException?.Message ?? ex.Message}")));
        }
    }
}
