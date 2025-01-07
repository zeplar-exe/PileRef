using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using MsBox.Avalonia;
using Newtonsoft.Json;
using Serilog;

namespace PileRef.Model.Document;

[JsonObject(MemberSerialization.OptIn)]
public class DocumentUri
{
    [JsonProperty("path")]
    public string Path { get; }
    
    public Uri Uri => new(Path);
    
    [JsonProperty("is_file")]
    public bool IsFile { get; }

    public DocumentUri(string path, bool isFile)
    {
        Path = path;
        IsFile = isFile;
    }
    
    public async Task<Stream?> OpenAsync()
    {
        Stream stream;
        
        if (IsFile)
        {
            stream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        else
        {
            var u = Uri;
            var response = await App.HttpClient.GetAsync(Uri);

            if (!response.IsSuccessStatusCode)
            {
                await MessageBoxManager.GetMessageBoxStandard(
                    "HTTP Request Failed", 
                    $"Status Code {(int)response.StatusCode}: {response.ReasonPhrase}. See full error message in {App.LogFileName}.")
                    .ShowAsync();
                
                Log.Logger.Error($"HTTP Request to {Path} Failed with {(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
                
                return null;
            }
            
            stream = await response.Content.ReadAsStreamAsync();
        }

        return stream;
    }
}