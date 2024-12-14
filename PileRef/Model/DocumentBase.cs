using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public abstract partial class DocumentBase : ObservableObject, IPileObject
{
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private DocumentUri uri;
    [ObservableProperty] private double xPosition;
    [ObservableProperty] private double yPosition;
    [ObservableProperty] private double width;
    [ObservableProperty] private double height;
    
    protected DocumentBase(DocumentUri uri)
    {
        Uri = uri;
    }

    protected static async Task<Stream> ReadUriAsync(DocumentUri uri)
    {
        Stream stream;
        
        if (uri.IsFile)
        {
            stream = File.OpenRead(uri.Path);
        }
        else
        {
            var response = await App.HttpClient.GetAsync(System.Uri.EscapeDataString(uri.Path));
            
            stream = await response.Content.ReadAsStreamAsync();
        }

        return stream;
    }
}