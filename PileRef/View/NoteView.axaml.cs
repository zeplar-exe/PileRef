using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using PileRef.Model;
using PileRef.ViewModel;

namespace PileRef.View
{
    public partial class NoteView : ObjectViewBase
    {
        public NoteViewModel ViewModel { get; }
        
        public Note Note
        {
            get => GetValue(NoteProperty); 
            set => SetValue(NoteProperty, value); 
        }
        
        public static readonly StyledProperty<Note> NoteProperty =
            AvaloniaProperty.Register<NoteView, Note>(nameof(Note));
        
        public override IPileObject PileObject => Note;
        
        public NoteView()
        {
            ViewModel = new NoteViewModel();
            DataContext = this;
            
            InitializeComponent();
            
            v_Title.AddHandler(KeyDownEvent, OnContentKeyDown, RoutingStrategies.Tunnel);
            v_Content.AddHandler(KeyDownEvent, OnContentKeyDown, RoutingStrategies.Tunnel);
        }

        public override void BeginInteract()
        {
            ViewModel.IsEditing = true;
        }

        public override void EndInteract()
        {
            ViewModel.IsEditing = false;
        }
        
        private void OnTitleKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    v_Content.Focus();
                    break;
                case Key.Escape:
                    TopLevel.GetTopLevel(this)!.Focus();
                    break;
            }
        }

        private void OnContentKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    TopLevel.GetTopLevel(this)!.Focus();
                    break;
            }
        }
    }
}