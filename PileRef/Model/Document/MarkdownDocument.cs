using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public partial class MarkdownDocument : TextDocumentBase
{
    public MarkdownDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
        
    }
}