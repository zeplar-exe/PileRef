using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class DocumentBase : ObservableObject, IDocument
{
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private DocumentUri uri;
    [ObservableProperty] private double xPosition;
    [ObservableProperty] private double yPosition;
    [ObservableProperty] private double width;
    [ObservableProperty] private double height;
    
    protected DocumentBase(DocumentUri uri)
    {
        Uri = uri;
    }
}