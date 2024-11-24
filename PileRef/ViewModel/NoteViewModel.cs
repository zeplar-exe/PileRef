using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;

namespace PileRef.ViewModel;

public partial class NoteViewModel : ObservableObject
{
    [ObservableProperty] private Note? note;
    [ObservableProperty] private bool editingTitle = true;
    [ObservableProperty] private bool editingContent = true;
    [ObservableProperty] private bool selected;
}