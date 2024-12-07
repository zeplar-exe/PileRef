using System;

namespace PileRef.Model;

public class DocumentUri
{
    public Uri Uri { get; }
    public bool IsFile { get; }

    public DocumentUri(Uri uri, bool isFile)
    {
        Uri = uri;
        IsFile = isFile;
    }
}