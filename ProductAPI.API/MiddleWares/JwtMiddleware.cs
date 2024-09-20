using ProductAPI.Core.Services;
using ProductAPI.Service.Authorization.Abstraction;

namespace ProductAPI.API.MiddleWares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtAuthenticationManager iJwtAuthenticationManager)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            string token = null;

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var parts = authorizationHeader.Split(" ");
                if (parts.Length > 1)
                {
                    token = parts[parts.Length - 1];
                }
            }

            var userId = iJwtAuthenticationManager.ValidateJwtToken(token);

            if (userId != null)
            {
                context.Items["User"] = userService.GetById(userId).Result;
            }


            await _next(context);
        }
    }
}
