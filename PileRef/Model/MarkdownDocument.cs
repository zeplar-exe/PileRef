using System.IO;
using System.Text;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class MarkdownDocument : TextDocumentBase
{
    public MarkdownDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}