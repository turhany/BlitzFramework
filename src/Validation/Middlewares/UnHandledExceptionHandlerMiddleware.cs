using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlitzFramework.Constants;
using BlitzFramework.Data.Models; 
using BlitzFramework.Validation.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json; 
// ReSharper disable IdentifierTypo

namespace BlitzFramework.Validation.Middlewares
{
    public class UnHandledExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnHandledExceptionHandlerMiddleware> _logger;

        public UnHandledExceptionHandlerMiddleware(ILogger<UnHandledExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var response = new BaseModel();
            try
            {
                await _next(context);
            }
            catch (FluentValidationException ex)
            {
                _logger.LogError(ex, ex.Message);
                response.ErrorCode = ((int)HttpStatusCode.BadRequest).ToString();
                response.Message = ex.ValidationResult.Errors.Select(p => p.ErrorMessage).FirstOrDefault();
                
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = FrameworkConstants.JsonContentType;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.ErrorCode = ((int)HttpStatusCode.InternalServerError).ToString();
                response.Message = FrameworkConstants.ServerErrorMessage;

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = FrameworkConstants.JsonContentType;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
