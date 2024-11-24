using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class SimpleTextDocument : ObservableObject, IDocument
{
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string content = "";
    [ObservableProperty] private string filePath = "";
    [ObservableProperty] private double xPosition;
    [ObservableProperty] private double yPosition;
    [ObservableProperty] private double width;
    [ObservableProperty] private double height;
}