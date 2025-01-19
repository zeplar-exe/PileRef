using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;
using PileRef.Model.Document;

namespace PileRef.ViewModel;

public partial class DocumentViewModel : ObservableObject
{
    [ObservableProperty] public partial bool IsInteracting { get; set; }
    [ObservableProperty] public partial bool CanNoteConvert { get; set; }
    [ObservableProperty] public partial bool CanSearchText { get; set; }
}