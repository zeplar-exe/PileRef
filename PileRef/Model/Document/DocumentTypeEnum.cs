using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace PileRef.Model.Document;

public static class DocumentTypeEnum
{
    public static readonly DocumentType PDF = new("pdf", "PDF", [".pdf"]);
    public static readonly DocumentType EPUB = new("epub", "EPUB", [".epub"]);
    public static readonly DocumentType DOC = new("doc", "DOC", [".doc"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType DOCX = new("docx", "DOCX", [".docx"]);
    public static readonly DocumentType ODT = new("odt", "ODT", [".odt"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType SVG = new("svg", "SVG", [".svg"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType RTF = new("rtf", "Rich Text", [".rtf"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType PAGES = new("apple_pages", "Apple Pages", [".pages"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType Latex = new("tex", "LaTeX", [".tex"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType Markdown = new("md", "Markdown", [".md", "markdown"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType PlainText = new("txt", "Plain Text", [".txt"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType XPS = new("xps", "XPS", [".xps"], 
        DocumentFlags.TextEncodable);
    public static readonly DocumentType Image = new("image", "Image", [".png", ".jpg", ".jpeg", ".bmp"]);

    public static readonly DocumentType[] Values = typeof(DocumentTypeEnum).GetFields()
        .Where(p => p.FieldType == typeof(DocumentType))
        .Select(p => (p.GetValue(null) as DocumentType)!).ToArray();

    public static DocumentType GetEnumFromType(Type type) 
    {
        if (!type.IsAssignableTo(typeof(DocumentBase)))
            throw new ArgumentException($"Type '{type.FullName}' is not assignable to '{typeof(DocumentBase).FullName}'");

        if (type == typeof(MarkdownDocument))
            return Markdown;
        if (type == typeof(PlainTextDocument))
            return PlainText;
        if (type == typeof(LatexDocument))
            return Latex;
        if (type == typeof(OldWordDocument))
            return DOC;
        if (type == typeof(OdtDocument))
            return ODT;
        if (type == typeof(SvgDocument))
            return SVG;
        if (type == typeof(PagesDocument))
            return PAGES;
        if (type == typeof(RichTextDocument))
            return RTF;
        if (type == typeof(XpsDocument))
            return XPS;
        if (type == typeof(PilePdfDocument))
            return PDF;
        if (type == typeof(EpubDocument))
            return EPUB;
        if (type == typeof(ImageDocument))
            return Image;
        if (type == typeof(XmlDocument))
            return DOCX;

        throw new ArgumentException($"Document type '{type.FullName}' is not handled'");
    }

    public static DocumentBase? CreateDocumentFromEnum(DocumentType type, DocumentUri uri, Encoding encoding)
    {
        if (type == Markdown) 
            return new MarkdownDocument(uri, encoding);
        if (type == PlainText) 
            return new PlainTextDocument(uri, encoding);
        if (type == Latex)
            return new LatexDocument(uri, encoding);
        if (type == DOC)
            return new OldWordDocument(uri, encoding);
        if (type == ODT)
            return new OdtDocument(uri);
        if (type == SVG)
            return new SvgDocument(uri, encoding);
        if (type == PAGES)
            return new PagesDocument(uri);
        // https://web.archive.org/web/20241120144313/https://www.tempmail.us.com/en/iwork/effortlessly-accessing-pages-and-numbers-files-with-c-on-windows
        if (type == RTF)
            return new RichTextDocument(uri, encoding);
        if (type == XPS)
            return new XpsDocument(uri);
        if (type == PDF)
            return new PilePdfDocument(uri);
        if (type == EPUB)
            return new EpubDocument(uri);
        if (type == Image)
            return new ImageDocument(uri);
        if (type == DOCX)
            return new XmlWordDocument(uri);

        return null;
    }
}