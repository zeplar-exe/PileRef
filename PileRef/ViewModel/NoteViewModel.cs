using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;

namespace PileRef.ViewModel;

public partial class NoteViewModel : ObservableObject
{
    [ObservableProperty] private Note? note;
    [ObservableProperty] private bool notEditingTitle = true;
    [ObservableProperty] private bool notEditingContent = true;
    [ObservableProperty] private bool selected;
}