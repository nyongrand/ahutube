using System;
using System.Text.Json.Serialization;

namespace ConsoleAhuTube
{
    public class Video
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("title")]
        public string Title { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("uploadDate")]
        public DateTime UploadDate { get; private set; }

        [JsonPropertyName("uploader")]
        public string Uploader { get; private set; }

        [JsonPropertyName("uploaderId")]
        public string UploaderId { get; private set; }

        [JsonPropertyName("duration")]
        public double Duration { get; private set; }

        [JsonPropertyName("viewCount")]
        public int ViewCount { get; private set; }

        [JsonPropertyName("views")]
        public string Views { get; private set; }

        [JsonPropertyName("averageRating")]
        public double AverageRating { get; private set; }

        [JsonPropertyName("categories")]
        public string[] Categories { get; private set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; private set; }

        [JsonPropertyName("likeCount")]
        public int LikeCount { get; private set; }

        [JsonPropertyName("likes")]
        public string Likes { get; private set; }

        [JsonPropertyName("dislikeCount")]
        public int DislikeCount { get; private set; }

        [JsonPropertyName("dislikes")]
        public string Dislikes { get; private set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; private set; }

        [JsonPropertyName("height")]
        public int Height { get; private set; }

        [JsonPropertyName("filename")]
        public string Filename { get; private set; }

        public Video(VideoData data)
        {
            Id = data.Id;
            Title = data.Title;
            Description = data.Description;
            UploadDate = data.UploadDate;
            Uploader = data.Uploader;
            UploaderId = data.UploaderId;
            Duration = data.Duration;
            ViewCount = data.ViewCount;
            Views = ViewsToString(data.ViewCount);
            AverageRating = data.AverageRating;
            Categories = data.Categories;
            Tags = data.Tags;
            LikeCount = data.LikeCount;
            Likes = ViewsToString(data.LikeCount);
            DislikeCount = data.DislikeCount;
            Dislikes = ViewsToString(data.DislikeCount);
            Thumbnail = data.Thumbnail;
            Width = data.Width;
            Height = data.Height;
            Filename = data.Filename;
        }

        private static string ViewsToString(int views)
        {
            if (views > 10_000_000)
                return $"{views / 1_000_000}M";

            if (views > 1_000_000)
                return $"{views / 1_000_000}.{views / 100_000 % 10}M";

            if (views > 10_000)
                return $"{views / 1_000}K";

            if (views > 1_000)
                return $"{views / 1_000}.{views / 100 % 10}K";

            return $"{views}";
        }
    }
}
