using System.IO;
using System.Text;

namespace PileRef.Model;

public class RichTextDocument : TextDocumentBase
{
    public RichTextDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}