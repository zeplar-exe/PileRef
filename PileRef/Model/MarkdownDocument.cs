using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class MarkdownDocument : TextDocumentBase
{
    public MarkdownDocument(string content, DocumentUri uri) : base(content, uri)
    {
        
    }
}