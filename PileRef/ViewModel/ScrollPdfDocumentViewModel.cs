using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using PdfiumViewer;
using PileRef.Model.Document;

namespace PileRef.ViewModel;

public partial class ScrollPdfDocumentViewModel : ObservableObject
{
    [ObservableProperty] public partial ObservableCollection<IImage> PageImages { get; set; } = [];

    public PilePdfDocument Document { get; }
    
    public ScrollPdfDocumentViewModel(PilePdfDocument document)
    {
        Document = document;

        Refresh();
        document.OnUpdated += (_, _) => Refresh();
    }

    private void Refresh()
    {
        PageImages.Clear();
        
        for (var index = 0; index < Document.Document.PageCount; index++)
        {
            var size = Document.Document.PageSizes[index].ToSize();
            var render = Document.Document.Render(index, 
                size.Width * 2, size.Height * 2, MainWindow.Dpi, MainWindow.Dpi, 
                PdfRenderFlags.None);

            using var memory = new MemoryStream();
        
            render.Save(memory, ImageFormat.Png);
            memory.Seek(0, SeekOrigin.Begin);
            
            PageImages.Add(new Bitmap(memory));
        }
    }
}