using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PileRef.Model;
using PileRef.ViewModel;

namespace PileRef
{
    public partial class DocumentView : UserControl
    {
        public DocumentViewModel ViewModel { get; }
        
        public IDocument Document
        {
            get => GetValue(DocumentProperty); 
            set => SetValue(DocumentProperty, value); 
        }
        
        public static readonly StyledProperty<IDocument> DocumentProperty =
            AvaloniaProperty.Register<DocumentView, IDocument>(nameof(Document));
        
        public static readonly RoutedEvent<PointerPressedEventArgs> SelectedEvent =
            RoutedEvent.Register<DocumentView, PointerPressedEventArgs>(nameof(Selected), RoutingStrategies.Direct);

        public event EventHandler<RoutedEventArgs> Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }
        
        public DocumentView()
        {
            ViewModel = new DocumentViewModel();
            DataContext = this;
            
            InitializeComponent();
            
            this.AddHandler(PointerPressedEvent, ControlPressed, RoutingStrategies.Tunnel);
            v_Title.AddHandler(PointerPressedEvent, PressedTitleEdit, RoutingStrategies.Tunnel);
            v_Title.AddHandler(LostFocusEvent, TitleEditLostFocus, RoutingStrategies.Tunnel);
        }
        
        private void ControlPressed(object? sender, PointerPressedEventArgs e)
        {
            var args = new PileObjectSelectedEventArgs(SelectedEvent, e);
            
            RaiseEvent(args);
        }

        private void PressedTitleEdit(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.EditingTitle = e.ClickCount >= 2;
        }

        private void TitleEditLostFocus(object? sender, RoutedEventArgs e)
        {
            ViewModel.EditingTitle = false;
        }
    }
}