using System.Diagnostics.CodeAnalysis;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace PileRef.Model.Document;

public sealed class ImageDocument : DocumentBase
{
    public IImage Image { get; private set; }

    public ImageDocument(DocumentUri uri) : base(uri)
    {
        
    }

    [MemberNotNull(nameof(Image))]
    public override void Update()
    {
        Image = new Bitmap(Stream);
    }
}