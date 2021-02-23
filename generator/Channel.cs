using System.Text.Json.Serialization;

namespace ConsoleAhuTube
{
    public class Channel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("folder")]
        public string Folder { get; set; }

        public Channel(Video video)
        {
            Id = video.UploaderId;
            Name = video.Uploader;
        }
    }
}
