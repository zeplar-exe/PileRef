using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class MarkdownDocument : ObservableObject, IDocument
{
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string content = "";
    [ObservableProperty] private double xPosition;
    [ObservableProperty] private double yPosition;
    [ObservableProperty] private double width;
    [ObservableProperty] private double height;
}