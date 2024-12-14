using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using PdfiumViewer;

namespace PileRef.Model;

public class PilePdfDocument : DocumentBase
{
    public Stream Stream { get; }
    public PdfDocument Document { get; }

    public static async Task<PilePdfDocument> Create(DocumentUri uri)
    {
        var stream = await ReadUriAsync(uri);

        return new PilePdfDocument(stream, uri);
    }
    
    protected PilePdfDocument(Stream stream, DocumentUri uri) : base(uri)
    {
        Stream = stream;
        Document = PdfDocument.Load(stream);
    }
}