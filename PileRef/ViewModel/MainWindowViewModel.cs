using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;

namespace PileRef.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private Pile? pile;
    [ObservableProperty] private string? pileFilePath;
    public ObservableCollection<string> RecentPiles { get; } = [];
    [ObservableProperty] private bool changesMade;

    partial void OnPileChanged(Pile? value)
    {
        ChangesMade = true;
    }
}