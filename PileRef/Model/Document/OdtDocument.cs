using System.IO;

namespace PileRef.Model.Document;

public class OdtDocument : DocumentBase
{
    public OdtDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}