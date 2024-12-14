using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.ViewModel;

public partial class PdfDocumentViewModel : ObservableObject
{
    [ObservableProperty] private bool usePaginateDisplay = true;
    [ObservableProperty] private bool useScrollDisplay;
}