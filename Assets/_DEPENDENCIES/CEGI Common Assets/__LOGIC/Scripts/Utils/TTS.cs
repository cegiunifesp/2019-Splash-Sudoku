using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TTS
{
    public static string CacheFolder = "TTSCache";
    public const string API_KEY = "0d446091a5b24a45af987c4624fd80af";

    public static void Get(string text, Action<AudioClip> onLoaded, int speed = 0, string idiom = "en-us", bool cache = true)
    {
        string fileName = ToFileName(text, idiom, speed.ToString());
        string cacheDir = Path.Combine(Application.streamingAssetsPath, CacheFolder);
        string path = Path.Combine(cacheDir, fileName);
        string url = $"http://api.voicerss.org/?key={API_KEY}&hl={idiom}&src={text}&r={speed}&c=WAV&f=48khz_16bit_stereo";

        if (!Directory.Exists(cacheDir))
            Directory.CreateDirectory(cacheDir);

        if (File.Exists(path))
        {
            url = $"file://{path}";

            CoroutineExecutor.Start(LoadSongCoroutine(url, onLoaded));
            return;
        }

        CoroutineExecutor.Start(LoadSongCoroutine(url, onLoaded, cache ? path : null));
    }

    public static void ClearCache()
    {
        string cacheDir = Path.Combine(Application.streamingAssetsPath, CacheFolder);
        if (Directory.Exists(cacheDir))
        {
            Directory.Delete(cacheDir, true);
        }
    }

    private static IEnumerator LoadSongCoroutine(string url, Action<AudioClip> onLoaded, string savePath = null)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        yield return www.SendWebRequest();
        if (www.isDone)
        {
            if (!string.IsNullOrWhiteSpace(savePath))
            {
                File.WriteAllBytes(savePath, www.downloadHandler.data);
            }

            onLoaded(DownloadHandlerAudioClip.GetContent(www));
        }
        else
        {
            Debug.LogError("Error getting TTS audio");
        }
    }

    private static string ToFileName(string text, string idiom, string speed)
    {
        var fileName = $"{text} {idiom} {speed}";
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '_');
        }

        return fileName;
    }
}
