using System.IO;
using System.Text;
using AvaloniaEdit.Document;

namespace PileRef.Model;

public class SvgDocument : TextDocumentBase
{
    public SvgDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}