using System.Linq;
using System.Reflection;

namespace PileRef.Model;

public static class DocumentTypeEnum
{
    public static readonly DocumentType PDF = new("PDF", [".pdf"]);
    public static readonly DocumentType EPUB = new("EPUB", [".epub"]);
    public static readonly DocumentType DOC = new("DOC", [".doc"], DocumentFlags.TextEncodable);
    public static readonly DocumentType DOCX = new("DOCX", [".docx"], DocumentFlags.TextEncodable);
    public static readonly DocumentType ODT = new("ODT", [".odt"], DocumentFlags.TextEncodable);
    public static readonly DocumentType SVG = new("SVG", [".svg"], DocumentFlags.TextEncodable);
    public static readonly DocumentType RTF = new("Rich Text", [".rtf"], DocumentFlags.TextEncodable);
    public static readonly DocumentType PAGES = new("Apple Pages", [".pages"], DocumentFlags.TextEncodable);
    public static readonly DocumentType Latex = new("LaTeX", [".tex"], DocumentFlags.TextEncodable);
    public static readonly DocumentType Markdown = new("Markdown", [".md", "markdown"], DocumentFlags.TextEncodable);
    public static readonly DocumentType PlainText = new("Plain Text", [".txt"], DocumentFlags.TextEncodable);
    public static readonly DocumentType XPS = new("XPS", [".xps"], DocumentFlags.TextEncodable);

    public static readonly DocumentType[] Values = typeof(DocumentTypeEnum).GetFields()
        .Where(p => p.FieldType == typeof(DocumentType))
        .Select(p => (p.GetValue(null) as DocumentType)!).ToArray();
}