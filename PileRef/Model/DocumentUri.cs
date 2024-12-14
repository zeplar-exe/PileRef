using System;

namespace PileRef.Model;

public class DocumentUri
{
    public string Path { get; }
    public bool IsFile { get; }

    public DocumentUri(string path, bool isFile)
    {
        Path = path;
        IsFile = isFile;
    }
}