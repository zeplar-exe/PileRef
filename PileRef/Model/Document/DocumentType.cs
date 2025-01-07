using System.Collections.Generic;

namespace PileRef.Model.Document;

public class DocumentType
{
    public string Id { get; }
    public string DisplayName { get; }
    public HashSet<string> Extensions { get; }
    public DocumentFlags Flags { get; }

    public DocumentType(string id, string displayName, string[] extensions, DocumentFlags flags = DocumentFlags.None)
    {
        Id = id;
        DisplayName = displayName;
        Extensions = new HashSet<string>(extensions);
        Flags = flags;
    }
}