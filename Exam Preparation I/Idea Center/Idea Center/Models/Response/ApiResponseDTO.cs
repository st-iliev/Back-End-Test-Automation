using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Idea_Center.Models.Response
{
    public class ApiResponseDTO
    {
        [JsonPropertyName("msg")]
        public string Message { get; set; }
        [JsonPropertyName("idea")]
        public IdeaDTO? Idea { get; set; }
    }
}
