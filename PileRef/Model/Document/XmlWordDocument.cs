using System.IO;

namespace PileRef.Model.Document;

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