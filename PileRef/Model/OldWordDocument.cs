using System.IO;
using System.Reflection.Metadata;
using System.Text;
using PileRef.Model;

namespace PileRef;

public class OldWordDocument : TextDocumentBase
{
    public OldWordDocument(Stream stream, DocumentUri uri, Encoding encoding) : base(stream, uri, encoding)
    {
    }
}