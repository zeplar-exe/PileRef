using System;

namespace PileRef.Model.Document;

[Flags]
public enum DocumentFlags
{
    None = 0,
    TextEncodable = 1<<0,
    Resizable = 1<<1
}