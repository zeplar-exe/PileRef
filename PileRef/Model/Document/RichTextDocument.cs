using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public class RichTextDocument : TextDocumentBase
{
    public RichTextDocument(DocumentUri uri, Encoding encoding) : base(uri, encoding)
    {
        
    }
}