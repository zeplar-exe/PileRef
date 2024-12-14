using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using PdfiumViewer;

namespace PileRef.Model;

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