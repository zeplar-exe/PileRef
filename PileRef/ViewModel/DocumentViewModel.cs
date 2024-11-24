using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;

namespace PileRef.ViewModel;

public partial class DocumentViewModel : ObservableObject
{
    [ObservableProperty] private bool editingTitle = true;
    [ObservableProperty] private bool selected;
}