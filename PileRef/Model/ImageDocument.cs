using System.IO;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace PileRef.Model;

public class ImageDocument : DocumentBase
{
    public IImage Image { get; private set; }

    public ImageDocument(Stream stream, DocumentUri uri) : base(uri, stream)
    {
        Image = new Bitmap(stream);
    }

    public override void Update()
    {
        Image = new Bitmap(Stream);
    }
}