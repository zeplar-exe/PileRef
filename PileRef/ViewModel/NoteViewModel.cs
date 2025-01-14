using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.ViewModel;

public partial class NoteViewModel : ObservableObject
{
    [ObservableProperty] private bool isEditing;
}