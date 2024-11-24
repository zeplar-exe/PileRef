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
        
        public NoteView()
        {
            ViewModel = new NoteViewModel();
            DataContext = ViewModel;
            
            InitializeComponent();
            
            v_Title.AddHandler(PointerPressedEvent, PressedTitleEdit, RoutingStrategies.Tunnel);
            v_Title.AddHandler(LostFocusEvent, TitleEditLostFocus, RoutingStrategies.Tunnel);
            v_Content.AddHandler(PointerPressedEvent, PressedContentEdit, RoutingStrategies.Tunnel);
            v_Content.AddHandler(LostFocusEvent, ContentEditLostFocus, RoutingStrategies.Tunnel);
        }
        
        private void PressedTitleEdit(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.NotEditingTitle = e.ClickCount < 2;
            Console.WriteLine(e.ClickCount);
        }

        private void TitleEditLostFocus(object? sender, RoutedEventArgs e)
        {
            ViewModel.NotEditingTitle = true;
        }
        
        private void PressedContentEdit(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.NotEditingContent = e.ClickCount < 2;
        }

        private void ContentEditLostFocus(object? sender, RoutedEventArgs e)
        {
            ViewModel.NotEditingContent = true;
        }
    }
}