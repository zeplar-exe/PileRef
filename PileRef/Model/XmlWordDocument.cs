using System.IO;
using System.Reflection.Metadata;

namespace PileRef.Model;

public class XmlWordDocument : DocumentBase
{
    public XmlWordDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}