using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
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

        protected override void OnBeginInteract()
        {
            ViewModel.IsEditing = true;
            v_Title.ClearSelection();
            v_Content.ClearSelection();
        }

        protected override void OnEndInteract()
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

        private void ContentOnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            e.Handled = true;
        }

        protected override void ControlPressed(object? sender, PointerPressedEventArgs e)
        {
            base.ControlPressed(sender, e);
            
            if (IsInteracting)
                return;
            
            if (!e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
                return;

            e.Handled = true;
            ContextMenu?.Open();
        }

        private async void SaveToFile(object? sender, RoutedEventArgs e)
        {
            var storageProvider = TopLevel.GetTopLevel(this)!.StorageProvider;

            await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions { SuggestedFileName = Note.Title });
        }

        private void Delete(object? sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RequestDeleteEvent, e));
        }
    }
}