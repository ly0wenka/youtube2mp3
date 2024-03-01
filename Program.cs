using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

class Program
{
    static async Task Main(string[] args)
    {
        var videoId = "https://www.youtube.com/watch?v=je5zd1nFdyk&ab_channel=Ado-Topic"; // Replace with the actual ID of the YouTube video you want to download
        var client = new YoutubeClient();
        var video = await client.Videos.GetAsync(videoId);
        var streamManifest = await client.Videos.Streams.GetManifestAsync(videoId);
        
        // Get the audio streams from the manifest
        var audioStreams = streamManifest.GetAudioOnlyStreams();
        
        // Find the audio stream with the highest bitrate
        var audioStreamInfo = audioStreams.OrderByDescending(s => s.Bitrate).First();
        
        // Get the actual stream
        var stream = await client.Videos.Streams.GetAsync(audioStreamInfo);

        var outputFilePath = video.Title + ".mp3";
        using (var outputFile = File.Create(outputFilePath))
        {
            await stream.CopyToAsync(outputFile);
        }
    }
}