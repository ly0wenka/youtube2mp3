using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

class Program
{
    static async Task Main(string[] args)
    {
        var videoId = args.Length > 1 ? args[1]:"https://www.youtube.com/shorts/133xQxW_74M";
        var client = new YoutubeClient();
        var video = await client.Videos.GetAsync(videoId);
        var streamManifest = await client.Videos.Streams.GetManifestAsync(videoId);
        
        // Get the audio streams from the manifest
        var audioStreams = streamManifest.GetAudioOnlyStreams();
        
        // Find the audio stream with the highest bitrate
        var audioStreamInfo = audioStreams.OrderByDescending(s => s.Bitrate).First();
        
        // Get the actual stream
        var stream = await client.Videos.Streams.GetAsync(audioStreamInfo);

        var outputFilePath = video.Title.Replace("/", "").Replace("\"","") + ".mp3";
        using (var outputFile = File.Create(outputFilePath))
        {
            await stream.CopyToAsync(outputFile);
        }
    }
}