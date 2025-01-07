using System.Diagnostics.CodeAnalysis;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace PileRef.Model.Document;

public sealed class ImageDocument : DocumentBase
{
    public IImage Image { get; private set; }

    public ImageDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        Update();
    }

    [MemberNotNull(nameof(Image))]
    public override void Update()
    {
        Image = new Bitmap(Stream);
    }
}