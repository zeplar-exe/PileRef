using System.IO;

namespace PileRef.Model.Document;

public class PagesDocument : DocumentBase
{
    public PagesDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}