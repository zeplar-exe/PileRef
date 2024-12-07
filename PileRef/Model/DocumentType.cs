using System.Collections.Generic;

namespace PileRef.Model;

public class DocumentType
{
    public string Name { get; }
    public HashSet<string> Extensions { get; }
    public DocumentFlags Flags { get; }

    public DocumentType(string name, string[] extensions, DocumentFlags flags = DocumentFlags.None)
    {
        Name = name;
        Extensions = new HashSet<string>(extensions);
        Flags = flags;
    }
}