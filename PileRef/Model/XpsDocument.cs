﻿using System.IO;

namespace PileRef.Model;

public class XpsDocument : DocumentBase
{
    public XpsDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}