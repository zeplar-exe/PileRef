using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class PlainTextDocument : TextDocumentBase
{
    public PlainTextDocument(string content, DocumentUri uri) : base(content, uri)
    {
        
    }
}