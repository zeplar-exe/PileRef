using System.IO;
using PdfiumViewer;

namespace PileRef.Model.Document;

public class PilePdfDocument : DocumentBase
{
    public PdfDocument Document { get; private set; }
    
    public PilePdfDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        Document = PdfDocument.Load(stream);
    }
    
    public override void Update()
    {
        Document = PdfDocument.Load(Stream);
    }
}