using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private void CreateNewNote(object? sender, PointerReleasedEventArgs e)
        {
            var menu = (sender as MenuItem)!;
            var pos = v_PileView.PointToClient(menu.PointToScreen(new Point(0, 0)));
            
            var note = new Note
            {
                Title = "Untitled Note",
                XPosition = pos.X,
                YPosition = pos.Y,
                Width = 280,
                Height = 300,
            };

            ViewModel.Pile ??= new Pile();
            ViewModel.Pile.Notes.Add(note);
        }

        private async void OpenDocument(object? sender, PointerReleasedEventArgs e)
        {
            var menu = (sender as MenuItem)!;
            var pos = v_PileView.PointToClient(menu.PointToScreen(new Point(0, 0)));
            
            var dialog = new OpenDocumentView();
            var document = await dialog.ShowDialog<IDocument?>(this);
            
            if (document == null)
                return;

            document.XPosition = pos.X;
            document.YPosition = pos.Y;
            document.Width = 280;
            document.Height = 300;

            ViewModel.Pile ??= new Pile();
            ViewModel.Pile.Documents.Add(document);
        }

        private void CreateNewPile(object? sender, PointerReleasedEventArgs e)
        {
            if (ViewModel.ChangesMade)
                ; // Warn

            ViewModel.Pile = new Pile();
            ViewModel.PileFilePath = null;
        }

        private async void OpenPile(object? sender, PointerReleasedEventArgs e)
        {
            var filter = new FileDialogFilter { Name = "PileRef Save File", Extensions = ["pile"] };
            var dialog = new OpenFileDialog();
            dialog.Filters.Add(filter);
            dialog.AllowMultiple = false;

            var result = await dialog.ShowAsync(this);
            
            if (result == null || result.Length == 0)
                return;

            var path = result[0];
            
            using var text = File.OpenText(path);
            await using var reader = new JsonTextReader(text);
            
            var json = await JToken.ReadFromAsync(reader);
            
            if (ViewModel.PileFilePath != null)
                ViewModel.RecentPiles.Add(ViewModel.PileFilePath);
            
            ViewModel.Pile = json.ToObject<Pile>();
            ViewModel.PileFilePath = path;
            ViewModel.ChangesMade = false;
        }

        private async void SavePile(object? sender, PointerReleasedEventArgs e)
        {
            ViewModel.Pile ??= new Pile();
            
            var filter = new FileDialogFilter { Name = "PileRef Save File", Extensions = ["pile"] };
            var dialog = new SaveFileDialog { DefaultExtension = ".pile" };
            dialog.Filters.Add(filter);

            var path = await dialog.ShowAsync(this);
            
            if (string.IsNullOrEmpty(path))
                return;
            
            var json = ViewModel.Pile.ToJson();

            await using var text = new StreamWriter(path);
            await using var writer = new JsonTextWriter(text);
            await json.WriteToAsync(writer);

            ViewModel.PileFilePath = path;
            ViewModel.ChangesMade = false;
        }
    }
}