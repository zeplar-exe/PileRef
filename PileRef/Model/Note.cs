using System;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class Note : ObservableObject, IPileObject
{
    [ObservableProperty] private string title;
    [ObservableProperty] private string text;
    [ObservableProperty] private double xPosition;
    [ObservableProperty] private double yPosition;
    [ObservableProperty] private double width;
    [ObservableProperty] private double height;
}