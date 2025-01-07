using System.IO;

namespace PileRef.Model.Document;

public class EpubDocument : DocumentBase
{
    public EpubDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}