using System.Text.Json.Serialization;

namespace ProductAPI.Core.DTOs
{
    public class NoContentDto
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }
    }
}
