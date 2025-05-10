using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Lütfen YouTube video URL'sini girin:");
        string videoUrl = Console.ReadLine();

        if (string.IsNullOrEmpty(videoUrl))
        {
            Console.WriteLine("Geçersiz video URL'si.");
            return;
        }

        var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync(videoUrl);
        string videoTitle = video.Title;

        string validTitle = CleanFileName(videoTitle);
        string mp3File = Path.Combine(@"C:\Users\TrySuphine\Downloads", validTitle + ".mp3");//Mp3ü kaydetmek istediğiniz dosya dizinini belirtilen alana giriniz ' @"C:\Users\TrySuphine\Downloads' gibi

        try
        {
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
            var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();
            await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, mp3File);
            Console.WriteLine("MP3 dosyası indirildi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata oluştu: {ex.Message}");
        }

        Console.WriteLine("İşlem tamamlandı. Çıkmak için bir tuşa basın.");
        Console.ReadLine();
    }

    static string CleanFileName(string title)
    {
        string cleanedTitle = Regex.Replace(title, @"[<>:""/\|?*]", "");
        return cleanedTitle;
    }
}
