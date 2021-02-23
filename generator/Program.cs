using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleAhuTube
{
    class Program
    {
        /// <summary>
        /// get channel list
        /// /channels.json
        /// 
        /// get channel details
        /// /channels/{channelId}.json
        /// 
        /// get channel videos list
        /// /channels/{channelId}/videos.json
        /// 
        /// get videos details
        /// /videos/{videoId}.json
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            var main = @"E:\Videos\AhuTube\";

            var channels = new List<Channel>();
            //var channels = new Dictionary<string, Channel>();
            foreach (var channelPath in Directory.GetDirectories(main))
            {
                var channelFolder = Path.GetFileName(channelPath);
                var jsonFolder = Path.Combine(main, channelFolder, "json");
                var thumbFolder = Path.Combine(main, channelFolder, "thumb");

                // checking if path contain json data
                if (!Directory.Exists(jsonFolder) || !Directory.Exists(thumbFolder))
                    continue;

                // create directory to copy original thumb to new thumbnail 
                var thumbnailFolder = Path.Combine(main, "data", "thumbnail");
                if (!Directory.Exists(thumbnailFolder))
                    Directory.CreateDirectory(thumbnailFolder);

                var videos = new List<Video>();
                foreach (var file in Directory.GetFiles(jsonFolder, "*"))
                {
                    var jsonContent = File.ReadAllText(file);
                    var video = new Video(JsonSerializer.Deserialize<VideoData>(jsonContent));

                    // get thumb filename and ext
                    var thumbnailFile = Directory.GetFiles(thumbFolder, $"*{video.Id}.*").First();
                    var thumbnailExt = Path.GetExtension(thumbnailFile);

                    // copy thumbnail file
                    var thumbnail = Path.Combine(thumbnailFolder, $"{video.Id}{thumbnailExt}");
                    File.Copy(thumbnailFile, thumbnail, true);

                    // rewrite thumbnail property
                    video.Thumbnail = $"{video.Id}{thumbnailExt}";

                    // write vide details to
                    // /videos/{videoId}.json
                    var videoJson = JsonSerializer.Serialize(video);
                    var videoJsonLocation = Path.Combine(main, "data", "videos", $"{video.Id}.json");
                    await WriteJsonAsync(videoJsonLocation, videoJson);

                    videos.Add(video);
                }

                // write this channel details
                // /channels/{channelId}.json
                var channel = new Channel(videos.First()) { Folder = channelFolder };
                var channelJson = JsonSerializer.Serialize(channel);
                var channelJsonLocation = Path.Combine(main, "data", "channels", $"{channel.Id}.json");
                await WriteJsonAsync(channelJsonLocation, channelJson);

                // write channel video list
                // /channels/{channelId}/videos.json
                var videosJson = JsonSerializer.Serialize(videos);
                var videosJsonLocation = Path.Combine(main, "data", "channels", $"{channel.Id}", "videos.json");
                await WriteJsonAsync(videosJsonLocation, videosJson);

                channels.Add(channel);
                //channels.Add(channel.Id, channel);
            }

            // write channel list
            // /channels.json
            var channelsJson = JsonSerializer.Serialize(channels);
            var channelsJsonLocation = Path.Combine(main, "data", "channels.json");
            await WriteJsonAsync(channelsJsonLocation, channelsJson);
        }

        /// <summary>
        /// write file, create directory if not exist
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        private static async Task WriteJsonAsync(string location, string json)
        {
            var directory = Path.GetDirectoryName(location);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await File.WriteAllTextAsync(location, json);
        }
    }
}
