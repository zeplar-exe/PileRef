using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public class SvgDocument : TextDocumentBase
{
    public SvgDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}