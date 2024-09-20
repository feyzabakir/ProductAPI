using ProductAPI.Core.DTOs.Authentication;

namespace ProductAPI.Service.Authorization.Abstraction
{
    public interface IJwtAuthenticationManager
    {
        AuthResponseDto Authenticate(string userName, string password);
        string? ValidateJwtToken(string token);  
    }
}

