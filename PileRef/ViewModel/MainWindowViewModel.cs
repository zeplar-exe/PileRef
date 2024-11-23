using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;

namespace PileRef.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private Pile? pile;
    [ObservableProperty] private string? pileFilePath;
    [ObservableProperty] private List<string> recentPiles = [];
    [ObservableProperty] private bool changesMade;
}