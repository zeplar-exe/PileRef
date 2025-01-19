using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public partial class PlainTextDocument : TextDocumentBase
{
    public PlainTextDocument(DocumentUri uri, Encoding encoding) : base(uri, encoding)
    {
        
    }
}