namespace ProductAPI.Service.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; } = "dotnet_panel_key_value_here_secret";
        public int RefreshTokenTTL { get; set; }
    }
}
