namespace PileRef.Model;

public class DocumentType
{
    public string Name { get; }
    public bool IsTextEncodable { get; }
    public bool HorizontalResizable { get; }

    public DocumentType(string name, bool isTextEncodable, bool horizontalResizable)
    {
        Name = name;
        IsTextEncodable = isTextEncodable;
        HorizontalResizable = horizontalResizable;
    }
}