using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;

namespace PileRef.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private Pile? pile;
    [ObservableProperty] private string? pileFilePath;
    public ObservableCollection<string> RecentPiles { get; } = [];
    [ObservableProperty] private bool changesMade;

    [ObservableProperty] private bool isPanning;
    [ObservableProperty] private StandardCursorType cursor = StandardCursorType.Arrow;

    [ObservableProperty] private double panX;
    [ObservableProperty] private double panY;
    [ObservableProperty] private double zoomLevel = 0; 
    
    public RelativePoint RenderOrigin = new(0.5, 0.5, RelativeUnit.Relative);
    public double ZoomScale => Math.Pow(1.2, ZoomLevel);
    public double DragScale => Math.Pow(1.2, -ZoomLevel);
    
    partial void OnPileChanged(Pile? value)
    {
        ChangesMade = true;
    }

    partial void OnIsPanningChanged(bool oldValue, bool newValue)
    {
        Cursor = newValue ? StandardCursorType.Arrow : StandardCursorType.Hand;
    }

    partial void OnZoomLevelChanged(double value)
    {
        OnPropertyChanged(nameof(ZoomScale));
        OnPropertyChanged(nameof(DragScale));
    }
}