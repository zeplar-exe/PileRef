using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PileRef.Model;

namespace PileRef.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private Pile? pile;
    [ObservableProperty] private string? pileFilePath;
    public ObservableCollection<string> RecentPiles { get; } = [];
    [ObservableProperty] private bool changesMade;

    [ObservableProperty] private bool isPanning;
    [ObservableProperty] private StandardCursorType cursor = StandardCursorType.Arrow;

    [ObservableProperty] private double panX;
    [ObservableProperty] private double panY;
    [ObservableProperty] private double zoomLevel = 0; 
    
    public RelativePoint RenderOrigin = new(0.5, 0.5, RelativeUnit.Relative);
    public double ZoomScale => Math.Pow(1.2, ZoomLevel);
    public double DragScale => Math.Pow(1.2, -ZoomLevel);
    
    partial void OnPileChanged(Pile? value)
    {
        ChangesMade = true;
    }

    partial void OnIsPanningChanged(bool oldValue, bool newValue)
    {
        Cursor = newValue ? StandardCursorType.Arrow : StandardCursorType.Hand;
    }

    partial void OnZoomLevelChanged(double value)
    {
        OnPropertyChanged(nameof(ZoomScale));
        OnPropertyChanged(nameof(DragScale));
    }
    
    public async void SavePile(IStorageProvider storageProvider)
    {
        Pile ??= new Pile();

        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            FileTypeChoices =
            [
                new FilePickerFileType("PileRef Save File") { Patterns = ["*.pile"] }
            ],
            DefaultExtension = ".pile"
        });
            
        if (file == null)
            return;

        var path = Uri.UnescapeDataString(file.Path.AbsolutePath);
        var json = Pile.ToJson();

        await using var text = new StreamWriter(path);
        await using var writer = new JsonTextWriter(text);
        await json.WriteToAsync(writer);

        PileFilePath = path;
        ChangesMade = false;
    }
    
    public void CreateNote(Point position)
    {
        var note = new Note
        {
            Title = "Untitled Note",
            XPosition = position.X,
            YPosition = position.Y,
            Width = 280,
            Height = 300,
        };

        Pile ??= new Pile();
        Pile.Notes.Add(note);
    }
    
    public async void OpenDocument(Point position, Window dialogOwner)
    {
        var dialog = new OpenDocumentView();
        var document = await dialog.ShowDialog<IDocument?>(dialogOwner);
            
        if (document == null)
            return;

        document.XPosition = position.X;
        document.YPosition = position.Y;
        document.Width = 280;
        document.Height = 300;

        Pile ??= new Pile();
        Pile.Documents.Add(document);
    }

    public void CreatePile()
    {
        Pile = new Pile();
        PileFilePath = null;
    }

    public async void OpenPile(IStorageProvider storageProvider)
    {
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            FileTypeFilter = [new FilePickerFileType("Pile Ref Save File") { Patterns = ["*.pile"] }],
            AllowMultiple = false,
        });

        if (files.Count != 1)
            return;

        var storageFile = files[0];

        await using var stream = await storageFile.OpenWriteAsync();
        using var text = new StreamReader(stream);
        await using var reader = new JsonTextReader(text);
            
        var json = await JToken.ReadFromAsync(reader);
            
        if (PileFilePath != null)
            RecentPiles.Add(PileFilePath);
            
        Pile = json.ToObject<Pile>();
        PileFilePath = Uri.UnescapeDataString(storageFile.Path.AbsolutePath);
        ChangesMade = false;
    }
}