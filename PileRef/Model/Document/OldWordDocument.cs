using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public class OldWordDocument : TextDocumentBase
{
    public OldWordDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
    }
}