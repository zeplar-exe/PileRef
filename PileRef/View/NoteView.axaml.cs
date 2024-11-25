using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
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
        
        public Note Note
        {
            get => GetValue(NoteProperty); 
            set => SetValue(NoteProperty, value); 
        }
        
        public static readonly StyledProperty<Note> NoteProperty =
            AvaloniaProperty.Register<NoteView, Note>(nameof(Note));
        
        public static readonly RoutedEvent<PointerPressedEventArgs> SelectedEvent =
            RoutedEvent.Register<NoteView, PointerPressedEventArgs>(nameof(Selected), RoutingStrategies.Direct);

        public event EventHandler<RoutedEventArgs> Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }
        
        public NoteView()
        {
            ViewModel = new NoteViewModel();
            DataContext = this;
            
            InitializeComponent();
            
            this.AddHandler(PointerPressedEvent, ControlPressed, RoutingStrategies.Tunnel);
            v_Title.AddHandler(PointerPressedEvent, PressedTitleEdit, RoutingStrategies.Tunnel);
            v_Title.AddHandler(KeyDownEvent, OnTextKeyDown, RoutingStrategies.Tunnel);
            v_Content.AddHandler(PointerPressedEvent, PressedContentEdit, RoutingStrategies.Tunnel);
            v_Content.AddHandler(KeyDownEvent, OnTextKeyDown, RoutingStrategies.Tunnel);
        }
        
        private void ControlPressed(object? sender, PointerPressedEventArgs e)
        {
            var args = new NoteSelectedEventArgs(SelectedEvent, e);
            
            RaiseEvent(args);
        }

        private void PressedTitleEdit(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.EditingTitle = ViewModel.EditingTitle || e.ClickCount >= 2;
        }

        private void TitleEditLostFocus(object? sender, RoutedEventArgs e)
        {
            ViewModel.EditingTitle = false;
            ViewModel.EditingContent = false;
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

        private void OnTextKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                TopLevel.GetTopLevel(this)!.Focus();
            }
        }
    }
}