using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using PileRef.Model;
using PileRef.ViewModel;

namespace PileRef
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; }
        
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            
            InitializeComponent();
        }

        private void CreateNewNote(object? sender, PointerPressedEventArgs e)
        {
            var menu = sender as MenuItem;
            var pos = v_PileView.PointToClient(menu.PointToScreen(new Point(0, 0)));
            
            var note = new Note
            {
                XPosition = pos.X,
                YPosition = pos.Y,
                Width = 280,
                Height = 300,
            };

            ViewModel.Pile ??= new Pile();
            ViewModel.Pile.Notes.Add(note);
        }

        private void CreateNewDocument(object? sender, PointerPressedEventArgs e)
        {
            // need dialog
            
            throw new System.NotImplementedException();
        }

        private void CreateNewPile(object? sender, PointerPressedEventArgs e)
        {
            if (ViewModel.ChangesMade)
                ; // Warn

            ViewModel.Pile = new Pile();
        }

        private async void OpenPile(object? sender, PointerPressedEventArgs e)
        {
            var filter = new FileDialogFilter { Name = "PileRef Save File", Extensions = ["pile"] };
            var dialog = new OpenFileDialog();
            dialog.Filters.Add(filter);
            dialog.AllowMultiple = false;

            var result = await dialog.ShowAsync(this);
            
            if (result == null || result.Length == 0)
                return;

            var path = result[0];
        }
    }
}