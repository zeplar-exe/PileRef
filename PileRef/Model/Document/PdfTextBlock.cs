using Avalonia;

namespace PileRef.Model.Document;

public class PdfTextBlock
{
    public string Text { get; }
    public Rect Rect { get; }

    public PdfTextBlock(string text, Rect rect)
    {
        Text = text;
        Rect = rect;
    }
}