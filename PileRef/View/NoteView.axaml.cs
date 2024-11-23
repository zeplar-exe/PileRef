using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PileRef.Model;
using PileRef.ViewModel;

namespace PileRef
{
    public partial class NoteView : UserControl
    { // just get rid of the vm?
        // thinking of handling "convert to document" via 
        //   left click to select -> right clcik -> convert selection to document
        public NoteViewModel ViewModel { get; }
        
        public NoteView()
        {
            ViewModel = new NoteViewModel();
            DataContext = ViewModel;
            
            InitializeComponent();
        }
    }
}