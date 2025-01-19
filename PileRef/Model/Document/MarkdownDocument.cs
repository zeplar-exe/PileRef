using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public partial class MarkdownDocument : TextDocumentBase
{
    public MarkdownDocument(DocumentUri uri, Encoding encoding) : base(uri, encoding)
    {
        
    }
}