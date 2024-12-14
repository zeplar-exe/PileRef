using System;
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
    
    protected Stream Stream { get; }
    
    protected DocumentBase(DocumentUri uri, Stream stream)
    {
        Uri = uri;
        Stream = stream;
        
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
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                throw;
            }
        };
    }

    public abstract void Update();
}