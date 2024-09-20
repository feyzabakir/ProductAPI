namespace ProductAPI.Core.DTOs.Authentication
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
