using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.ViewModel;

public partial class NoteViewModel : ObservableObject
{
    [ObservableProperty] private bool editingTitle;
    [ObservableProperty] private bool editingContent;
}