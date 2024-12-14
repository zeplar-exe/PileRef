using System.IO;
using System.Text;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class PlainTextDocument : TextDocumentBase
{
    public PlainTextDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}