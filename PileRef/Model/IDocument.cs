using Avalonia;

namespace PileRef.Model;

public interface IDocument
{
    public string Title { get; set; }
    public double XPosition { get; set; }
    public double YPosition { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}