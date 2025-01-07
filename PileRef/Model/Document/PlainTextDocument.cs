using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public partial class PlainTextDocument : TextDocumentBase
{
    public PlainTextDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}