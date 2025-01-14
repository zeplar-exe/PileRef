using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using PileRef.Model;
using PileRef.ViewModel;
using DocumentBase = PileRef.Model.Document.DocumentBase;

namespace PileRef.View
{
    public partial class DocumentView : ObjectViewBase
    {
        public DocumentViewModel ViewModel { get; }
        
        public DocumentBase Document
        {
            get => GetValue(DocumentProperty); 
            set => SetValue(DocumentProperty, value); 
        }
        
        public static readonly StyledProperty<DocumentBase> DocumentProperty =
            AvaloniaProperty.Register<DocumentView, DocumentBase>(nameof(Document));
        
        public override IPileObject PileObject => Document;
        
        public DocumentView()
        {
            ViewModel = new DocumentViewModel();
            DataContext = this;
            
            InitializeComponent();
            
            AddHandler(PointerPressedEvent, ControlPressed, RoutingStrategies.Tunnel);
            v_Title.AddHandler(PointerPressedEvent, PressedTitleEdit, RoutingStrategies.Tunnel);
            v_Title.AddHandler(LostFocusEvent, TitleEditLostFocus, RoutingStrategies.Tunnel);
        }

        public override void BeginInteract()
        {
            
        }

        public override void EndInteract()
        {
            
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