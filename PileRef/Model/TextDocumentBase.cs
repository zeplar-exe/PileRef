using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public abstract partial class TextDocumentBase : DocumentBase
{
    [ObservableProperty] private string content;
    [ObservableProperty] private Encoding encoding;
    
    protected TextDocumentBase(Stream stream, DocumentUri uri, Encoding encoding) : base(uri, stream)
    {
        Encoding = encoding;
        
        Update();
    }

    public sealed override async void Update()
    {
        Stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(Stream, Encoding);
        
        Content = await reader.ReadToEndAsync();
    }
}