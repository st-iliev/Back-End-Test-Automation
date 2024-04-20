using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Idea_Center.Models.Response
{
    public class IdeaDTO
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("url")]
        public string URL { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
