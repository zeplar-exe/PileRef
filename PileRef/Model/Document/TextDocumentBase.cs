using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PileRef.Model.Document;

[JsonObject(MemberSerialization.OptIn)]
public abstract partial class TextDocumentBase : DocumentBase
{
    [ObservableProperty] [JsonProperty("content")]
    public partial string Content { get; set; }

    [ObservableProperty] [JsonProperty("encoding")]
    public partial Encoding Encoding { get; set; }

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