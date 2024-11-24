namespace PileRef.Model;

public static class DocumentTypeEnum
{
    public static DocumentType Markdown = new(nameof(Markdown), true, true);
    public static DocumentType PlainText = new(nameof(PlainText), true, true);
    public static DocumentType Binary = new(nameof(Binary), false, true);
    public static DocumentType Hexadecimal = new(nameof(Hexadecimal), false, true);
    
    public static DocumentType[] Values =
    [
        Markdown, PlainText, Binary, Hexadecimal
    ];
}