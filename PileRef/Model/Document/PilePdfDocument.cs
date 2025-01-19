using System.IO;
using PdfiumViewer;

namespace PileRef.Model.Document;

public class PilePdfDocument : DocumentBase
{
    public PdfDocument Document { get; private set; }
    
    public PilePdfDocument(DocumentUri uri) : base(uri)
    {
        
    }
    
    public override void Update()
    {
        Document = PdfDocument.Load(Stream);
    }
}