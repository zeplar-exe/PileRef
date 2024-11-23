using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class Pile : ObservableObject
{
    [ObservableProperty] private ObservableCollection<IDocument> documents = [];
    [ObservableProperty] private ObservableCollection<Note> notes = [];
}