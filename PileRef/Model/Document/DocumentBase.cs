using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PileRef.Model.Document;

[JsonObject(MemberSerialization.OptIn)]
public abstract partial class DocumentBase : ObservableObject, IPileObject
{
    [ObservableProperty] [JsonProperty("title")]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty] [JsonProperty("uri")]
    public partial DocumentUri Uri { get; set; }

    [ObservableProperty] [JsonProperty("x")]
    public partial double XPosition { get; set; }

    [ObservableProperty] [JsonProperty("y")]
    public partial double YPosition { get; set; }

    [ObservableProperty] [JsonProperty("width")]
    public partial double Width { get; set; } = 280;

    [ObservableProperty] [JsonProperty("height")]
    public partial double Height { get; set; } = 300;

    public event EventHandler? OnUpdated;
    
    protected Stream Stream { get; }
    
    protected DocumentBase(DocumentUri uri, Stream stream)
    {
        Uri = uri;
        Stream = stream;
        
        if (!Uri.IsFile)
            return;
        
        var watcher = new FileSystemWatcher
        {
            Path = Path.GetDirectoryName(uri.Path)!, Filter = Path.GetFileName(uri.Path),
            EnableRaisingEvents = true
        };
            
        watcher.Changed += (_, args) =>
        {
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                Update();
                OnUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (IOException e)
            {
                Task.Run(async () =>
                {
                    await MessageBoxManager.GetMessageBoxStandard(
                            "Document Read Error",
                            $"An error occured while updating {Title}: {e}")
                        .ShowAsync();
                });
            }
        };
    }

    public abstract void Update();
}