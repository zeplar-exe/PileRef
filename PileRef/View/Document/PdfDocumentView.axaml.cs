using Avalonia;
using Avalonia.Controls;
using PileRef.Model;
using PileRef.ViewModel;

namespace PileRef.View.Document;

public partial class PdfDocumentView : UserControl
{
    public static readonly StyledProperty<PilePdfDocument> DocumentProperty = AvaloniaProperty.Register<PdfDocumentView, PilePdfDocument>(
        nameof(Document));

    public PilePdfDocument Document
    {
        get => GetValue(DocumentProperty);
        set => SetValue(DocumentProperty, value);
    }
    
    public PdfDocumentView()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
        if (change.Property != DocumentProperty)
            return;
        
        if (change.NewValue == null)
            Content = null;
        else if (true)
            Content = new PaginatedPdfDocumentViewModel((change.NewValue as PilePdfDocument)!, this);
    }
}