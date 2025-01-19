using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace PileRef.Model.Document;

[JsonObject(MemberSerialization.OptIn)]
public abstract partial class DocumentBase : ObservableObject, IPileObject, IDisposable, IAsyncDisposable
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
    
    [ObservableProperty] public partial bool ShowTitle { get; set; } = true;

    public event EventHandler? OnUpdated;
    
    protected Stream Stream { get; private set; }
    private FileSystemWatcher? Watcher { get; set; }
    
    protected DocumentBase(DocumentUri uri)
    {
        Uri = uri;
    }

    async partial void OnUriChanged(DocumentUri value)
    {
        var stream = await value.OpenAsync();

        if (stream != null)
        {
            Stream = stream;
        }
        else
        {
            Log.Warning($"Failed to open stream for URI {value.Path}.");
            
            stream = new MemoryStream();
        }
        
        if (!Uri.IsFile)
            return;
        
        Watcher?.Dispose();
        Watcher = new FileSystemWatcher
        {
            Path = Path.GetDirectoryName(Uri.Path)!, Filter = Path.GetFileName(Uri.Path),
            EnableRaisingEvents = true
        };
        
        Watcher.Changed += (_, args) =>
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
        
        Update();
    }

    public void CopyTo(Stream target)
    {
        var head = Stream.Position;
        
        Stream.Seek(0, SeekOrigin.Begin);
        Stream.CopyTo(target);
        
        Stream.Seek(head, SeekOrigin.Begin);
    }

    public abstract void Update();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await Stream.DisposeAsync();
    }
}