using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

class Program
{
    static async Task Main(string[] args)
    {
        var videoId = "https://youtu.be/DD6o3f2-lVI"; // Replace with the actual ID of the YouTube video you want to download
        var client = new YoutubeClient();
        var video = await client.Videos.GetAsync(videoId);
        var streamInfoSet = await client.Videos.GetMediaStreamInfosAsync(videoId);
        var audioStreamInfo = streamInfoSet.Audio.WithHighestBitrate();
        var audioStream = await client.Videos.GetVideoMediaStreamInfosAsync(audioStreamInfo);

        var outputFilePath = video.Title + ".mp3";
        using (var outputFile = File.Create(outputFilePath))
        {
            await audioStream.CopyToAsync(outputFile);
        }
    }
}