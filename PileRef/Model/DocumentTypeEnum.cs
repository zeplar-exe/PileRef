namespace PileRef.Model;

public static class DocumentTypeEnum
{
    public static DocumentType PDF = new("PDF", [".pdf"]);
    public static DocumentType EPUB = new("EPUB", [".epub"]);
    public static DocumentType DOC = new("DOC", [".doc"], DocumentFlags.TextEncodable);
    public static DocumentType DOCX = new("DOCX", [".docx"], DocumentFlags.TextEncodable);
    public static DocumentType ODT = new("ODT", [".odt"], DocumentFlags.TextEncodable);
    public static DocumentType SVG = new("SVG", [".svg"], DocumentFlags.TextEncodable);
    public static DocumentType RTF = new("Rich Text", [".rtf"], DocumentFlags.TextEncodable);
    public static DocumentType PAGES = new("Apple Pages", [".pages"], DocumentFlags.TextEncodable);
    public static DocumentType Latex = new("LaTeX", [".tex"], DocumentFlags.TextEncodable);
    public static DocumentType Html = new("Web Page", [".htm", ".html"], DocumentFlags.TextEncodable);
    public static DocumentType Markdown = new("Markdown", [".md", "markdown"], DocumentFlags.TextEncodable);
    public static DocumentType PlainText = new("Plain Text", [".txt"], DocumentFlags.TextEncodable);
    public static DocumentType XPS = new("XPS", [".xps"], DocumentFlags.TextEncodable);
    
    public static DocumentType[] Values =
    [
        Markdown, PlainText,
        PDF, EPUB, DOC, DOCX, ODT, RTF, PAGES, Latex, Html
    ];
}