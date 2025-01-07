using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public class LatexDocument : TextDocumentBase
{
    public LatexDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}