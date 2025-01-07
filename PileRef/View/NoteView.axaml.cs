using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using PileRef.Model;
using PileRef.ViewModel;

namespace PileRef.View
{
    public partial class NoteView : ObjectViewBase<NoteView>
    {
        public NoteViewModel ViewModel { get; }
        
        public Note Note
        {
            get => GetValue(NoteProperty); 
            set => SetValue(NoteProperty, value); 
        }
        
        public static readonly StyledProperty<Note> NoteProperty =
            AvaloniaProperty.Register<NoteView, Note>(nameof(Note));
        
        public NoteView()
        {
            ViewModel = new NoteViewModel();
            DataContext = this;
            
            InitializeComponent();
            
            v_Title.AddHandler(PointerPressedEvent, PressedTitleEdit, RoutingStrategies.Tunnel);
            v_Title.AddHandler(KeyDownEvent, OnContentKeyDown, RoutingStrategies.Tunnel);
            v_Content.AddHandler(PointerPressedEvent, PressedContentEdit, RoutingStrategies.Tunnel);
            v_Content.AddHandler(KeyDownEvent, OnContentKeyDown, RoutingStrategies.Tunnel);
        }

        private void PressedTitleEdit(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.EditingTitle = ViewModel.EditingTitle || e.ClickCount >= 2;
        }

        private void TitleEditLostFocus(object? sender, RoutedEventArgs e)
        {
            if (!ViewModel.EditingContent)
            {
                ViewModel.EditingTitle = false;
                ViewModel.EditingContent = false;
            }
        }
        
        private void PressedContentEdit(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.EditingContent = ViewModel.EditingContent || e.ClickCount >= 2;
        }

        private void ContentEditLostFocus(object? sender, RoutedEventArgs e)
        {
            ViewModel.EditingTitle = false;
            ViewModel.EditingContent = false;
        }
        
        private void OnTitleKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    ViewModel.EditingTitle = false;
                    ViewModel.EditingContent = true;
                    v_Content.IsReadOnly = false;
                    v_Content.Focusable = true;
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