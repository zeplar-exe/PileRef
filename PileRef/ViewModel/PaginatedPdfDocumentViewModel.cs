using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PdfiumViewer;
using PileRef.Model;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace PileRef.ViewModel;

public partial class PaginatedPdfDocumentViewModel : ObservableObject
{
    [ObservableProperty] private IImage? currentPageImage;
    [ObservableProperty] private int currentPageIndex;
    [ObservableProperty] private int currentPageCount;
    public ObservableCollection<PdfTextBlock> TextBlocks { get; } = [];
    
    private Control Control { get; }
    
    public PilePdfDocument Document { get; }

    public string CurrentPageFormatted => $"{CurrentPageIndex + 1}/{CurrentPageCount}";

    public PaginatedPdfDocumentViewModel(PilePdfDocument document, Control control)
    {
        Document = document;
        Control = control;
        CurrentPageCount = document.Document.PageCount;
        Refresh();
    }

    public void Refresh()
    {
        var size = Document.Document.PageSizes[CurrentPageIndex].ToSize();
        var textParts = Document.Document.GetPdfText(0).Split("\r\n");
        
        var render = Document.Document.Render(CurrentPageIndex, 
            size.Width * 2, size.Height * 2, MainWindow.Dpi, MainWindow.Dpi, 
            PdfRenderFlags.None);

        using var memory = new MemoryStream();
        
        render.Save(memory, ImageFormat.Png);
        memory.Seek(0, SeekOrigin.Begin);
            
        CurrentPageImage = new Bitmap(memory);
        
        return; // :)
        
        var xScale = (1.0/size.Width) * Control.Bounds.Width;
        var yScale = (1.0/size.Height) * Control.Bounds.Height;
        
        TextBlocks.Clear();

        var offset = 0;
        
        foreach (var part in textParts)
        {
            var span = new PdfTextSpan(CurrentPageIndex, offset, part.Length);
            var rect = Document.Document.GetTextBounds(span);

            if (rect.Count == 0)
                continue;
            
            var x = rect.MinBy(r => r.Bounds.X).Bounds.X * xScale;
            var y = rect.MinBy(r => r.Bounds.Y).Bounds.Y * yScale;
            var width = rect.MaxBy(r => r.Bounds.Width).Bounds.Width * xScale;
            var height = Math.Abs(rect.MaxBy(r => r.Bounds.Height).Bounds.Height) * yScale;
            
            TextBlocks.Add(new PdfTextBlock(part, new Rect(x, y, width, height)));
            
            offset += part.Length;
        }
    }
    
    [RelayCommand]
    public void NextPage()
    {
        if (CurrentPageIndex + 1 < Document.Document.PageCount)
            CurrentPageIndex++;
    }

    [RelayCommand]
    public void PreviousPage()
    {
        if (CurrentPageIndex > 0)
            CurrentPageIndex--;
    }

    partial void OnCurrentPageIndexChanged(int value)
    {
        Refresh();
        OnPropertyChanged(nameof(CurrentPageFormatted));
    }
}