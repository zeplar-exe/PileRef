using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PileRef.Model;
using PileRef.Model.Document;
using PileRef.Model.UndoRedo;
using PileRef.View;
using DocumentBase = PileRef.Model.Document.DocumentBase;

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
    
    private ActionManager ActionManager { get; }
    
    private IStorageProvider StorageProvider { get; }
    public double ZoomScale => Math.Pow(1.2, ZoomLevel);
    public double DragScale => Math.Pow(1.2, -ZoomLevel);

    public MainWindowViewModel(IStorageProvider storageProvider)
    {
        StorageProvider = storageProvider;
        ActionManager = new ActionManager();
    }
    
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

    [RelayCommand]
    public async Task SavePile()
    {
        if (ChangesMade || PileFilePath == null)
            await SavePileAs();
        else
        {
            var json = Pile!.ToJson();
            
            await using var text = new StreamWriter(PileFilePath);
            await using var writer = new JsonTextWriter(text);
            await json.WriteToAsync(writer);
        }
    }
    
    [RelayCommand]
    public async Task SavePileAs()
    {
        Pile ??= new Pile();

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
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
            XPosition = position.X,
            YPosition = position.Y
        };

        Pile ??= new Pile();
        Pile.Notes.Add(note);

        ActionManager.AddAction(new CreatePileObjectAction(Pile, note));
    }
    
    public async void OpenDocument(Point position, Window dialogOwner)
    {
        var dialog = new OpenDocumentView();
        var document = await dialog.ShowDialog<DocumentBase?>(dialogOwner);
            
        if (document == null)
            return;

        document.XPosition = position.X;
        document.YPosition = position.Y;

        Pile ??= new Pile();
        Pile.Documents.Add(document);
        
        ActionManager.AddAction(new CreatePileObjectAction(Pile, document));
    }

    [RelayCommand]
    public async Task CreatePile()
    {
        if (ChangesMade)
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(
                "Create New Pile", "The current pile has unsaved changes. Would you like to save?",
                ButtonEnum.YesNoCancel).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                await SavePile();
            }
            else if (result != ButtonResult.No)
            {
                return;
            }
        }
        
        Pile = new Pile();
        PileFilePath = null;
    }

    [RelayCommand]
    public async Task OpenPile()
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            FileTypeFilter = [new FilePickerFileType("PileRef Save File") { Patterns = ["*.pile"] }],
            AllowMultiple = false,
        });

        if (files.Count != 1)
            return;

        var storageFile = files[0];

        await using var stream = await storageFile.OpenWriteAsync();
        using var text = new StreamReader(stream);
        await using var reader = new JsonTextReader(text);
            
        var json = await JToken.ReadFromAsync(reader);

        if (json is not JObject jObject)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "Invalid Pile", "The selected pile is not correctly formatted and could not be opened.")
                .ShowAsync();
            
            return;
        }
        
        if (PileFilePath != null)
            RecentPiles.Add(PileFilePath);
        
        Pile = await Pile.FromJsonAsync(jObject);
        PileFilePath = Uri.UnescapeDataString(storageFile.Path.AbsolutePath);
        ChangesMade = false;
    }

    [RelayCommand(CanExecute = nameof(CanUndo))]
    public void Undo() => ActionManager.Undo();
    public bool CanUndo() => ActionManager.CanUndo;

    [RelayCommand(CanExecute = nameof(CanRedo))]
    public void Redo() => ActionManager.Redo();
    public bool CanRedo() => ActionManager.CanRedo;
}