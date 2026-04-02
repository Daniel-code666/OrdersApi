using Orders.Application.Common;
using Orders.Application.Common.Enum;
using System.Net;
using System.Text.Json;

namespace Orders.Api.Middleware
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Se produjo una excepción de negocio controlada.");

                await HandleBusinessExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se produjo una excepción no controlada.");

                await HandleUnhandledExceptionAsync(context);
            }
        }

        private static Task HandleBusinessExceptionAsync(HttpContext context, BusinessException exception)
        {
            HttpStatusCode status_code = GetStatusCode(exception.ErrorType);

            ErrorResponse response = new()
            {
                Code = (int)exception.ErrorType,
                Error = exception.ErrorType.ToString(),
                Message = GetErrorMessage(exception)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status_code;

            string json_response = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json_response);
        }

        private static Task HandleUnhandledExceptionAsync(HttpContext context)
        {
            ErrorResponse response = new()
            {
                Code = 500,
                Error = "InternalServerError",
                Message = "Ocurrió un error interno en el servidor."
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string json_response = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json_response);
        }

        private static HttpStatusCode GetStatusCode(ErrorType error_type)
            => error_type switch
            {
                ErrorType.OrdenNoEncontrada => HttpStatusCode.NotFound,
                ErrorType.OrdenNoCreada => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.BadRequest
            };

        private static string GetErrorMessage(BusinessException exception)
        {
            if (!string.IsNullOrWhiteSpace(exception.Message) &&
                exception.Message != exception.ErrorType.ToString())
            {
                return exception.Message;
            }

            return exception.ErrorType switch
            {
                ErrorType.OrdenNoEncontrada => "La orden no fue encontrada.",
                ErrorType.OrdenNoCreada => "La orden ingresada no es válida.",
                _ => "Se produjo un error de negocio."
            };
        }
    }
}
