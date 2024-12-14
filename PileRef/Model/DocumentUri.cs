using System;
using System.IO;
using System.Threading.Tasks;

namespace PileRef.Model;

public class DocumentUri
{
    public string Path { get; }
    public bool IsFile { get; }

    public DocumentUri(string path, bool isFile)
    {
        Path = path;
        IsFile = isFile;
    }
    
    public async Task<Stream> OpenAsync()
    {
        Stream stream;
        
        if (IsFile)
        {
            stream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        else
        {
            var response = await App.HttpClient.GetAsync(System.Uri.EscapeDataString(Path));
            
            stream = await response.Content.ReadAsStreamAsync();
        }

        return stream;
    }
}