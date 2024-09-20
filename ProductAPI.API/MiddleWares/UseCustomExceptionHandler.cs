using Microsoft.AspNetCore.Diagnostics;
using ProductAPI.Core.DTOs;
using ProductAPI.Service.Exceptions;
using System.Text.Json;

namespace ProductAPI.API.MiddleWares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {

                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var statusCode = exceptionFeature.Error switch
                    {
                        UnauthorizedAccessException => 401,  
                        ForbiddenAccessException => 403,    
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;


                    var response = GlobalResultDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);


                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                });
            });

        }
    }
}
