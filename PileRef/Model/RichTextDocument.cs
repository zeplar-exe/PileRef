using AvaloniaEdit.Document;

namespace PileRef.Model;

public class RichTextDocument : TextDocumentBase
{
    public RichTextDocument(string content, DocumentUri uri) : base(content, uri)
    {
        
    }
}