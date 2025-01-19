using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.ViewModel;

public partial class NoteViewModel : ObservableObject
{
    [ObservableProperty] public partial bool IsEditing { get; set; }
}