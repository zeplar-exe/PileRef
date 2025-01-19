using System.IO;
using System.Text;

namespace PileRef.Model.Document;

public class OldWordDocument : TextDocumentBase
{
    public OldWordDocument(DocumentUri uri, Encoding encoding) : base(uri, encoding)
    {
    }
}