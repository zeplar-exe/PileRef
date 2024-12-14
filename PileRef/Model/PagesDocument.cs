using System.IO;
using PileRef.Model;

namespace PileRef;

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