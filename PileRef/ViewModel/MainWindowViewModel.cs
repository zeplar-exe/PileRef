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
using Serilog;
using DocumentBase = PileRef.Model.Document.DocumentBase;

namespace PileRef.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private partial Pile? Pile { get; set; }

    [ObservableProperty] public partial string? PileFilePath { get; set; }
    public ObservableCollection<string> RecentPiles { get; } = [];
    [ObservableProperty] public partial bool ChangesMade { get; set; }

    [ObservableProperty] public partial bool IsPanning { get; set; }
    [ObservableProperty] public partial StandardCursorType Cursor { get; set; } = StandardCursorType.Arrow;

    [ObservableProperty] public partial double PanX { get; set; }
    [ObservableProperty] public partial double PanY { get; set; }
    [ObservableProperty] public partial double ZoomLevel { get; set; } = 0;
    
    [ObservableProperty] public partial IBrush SelectionBrush { get; set; } = new SolidColorBrush(Colors.LightBlue);
    [ObservableProperty] public partial double SelectionWidth { get; set; }
    [ObservableProperty] public partial double SelectionHeight { get; set; }
    [ObservableProperty] public partial double SelectionLeft { get; set; }
    [ObservableProperty] public partial double SelectionTop { get; set; }
    
    private ActionManager ActionManager { get; } = new();
    
    private IStorageProvider StorageProvider { get; }
    public double ZoomScale => Math.Pow(1.2, ZoomLevel);
    public double DragScale => Math.Pow(1.2, -ZoomLevel);
    
    public ObservableCollection<IPileObject> PileObjects { get; } = [];

    public MainWindowViewModel(IStorageProvider storageProvider)
    {
        StorageProvider = storageProvider;
    }
    
    partial void OnPileChanged(Pile? value)
    {
        ChangesMade = true;
        ZoomLevel = 0;
        PileObjects.Clear();
        
        if (value == null)
            return;
        
        foreach (var note in value.Notes)
            PileObjects.Add(note);
        foreach (var document in value.Documents)
            PileObjects.Add(document);
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

    public void Zoom(double levelChange)
    {
        if (Pile == null)
            return;
        
        ZoomLevel = Math.Min(ZoomLevel + levelChange, 15);
    }

    public void Pan(double x, double y)
    {
        if (Pile == null)
            return; 
        
        PanX += x;
        PanY += y;
    }
    
    public async Task<bool> HandleClosing()
    {
        if (!await RequestSaveChangesIfUnsaved())
            return false;
        
        Pile?.Dispose();

        return true;
    }

    [RelayCommand]
    public async Task SavePile()
    {
        if (PileFilePath == null)
        {
            await SavePileAs();
        }
        else if (ChangesMade)
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

        AddPileObject(note);

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

        AddPileObject(document);
        
        ActionManager.AddAction(new CreatePileObjectAction(Pile, document));
    }

    [RelayCommand]
    public async Task CreatePile()
    {
        if (!(await RequestSaveChangesIfUnsaved()))
            return;
        
        Pile = null;
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

        await using var stream = await storageFile.OpenReadAsync();
        using var text = new StreamReader(stream);
        await using var reader = new JsonTextReader(text);

        JObject jObject;

        try
        {
            var json = await JToken.ReadFromAsync(reader);

            if (json is not JObject jo)
            {
                await MessageBoxManager.GetMessageBoxStandard(
                        "Invalid Pile", "The selected pile is not correctly formatted and could not be opened.")
                    .ShowAsync();
                Log.Error($"Expected save JSON to take form of JSON object, got {json.Type}");

                return;
            }

            jObject = jo;
        }
        catch (JsonReaderException e)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Invalid Pile", "Failed to read pile JSON.")
                .ShowAsync();
            Log.Error($"({storageFile.Path})" +  e.Message);
            
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

    [MemberNotNull(nameof(Pile))]
    public void AddPileObject(IPileObject pileObject)
    {
        Pile ??= new Pile();
        
        PileObjects.Add(pileObject);
        
        if (pileObject is Note note)
        {
            Pile.Notes.Add(note);
        }
        else if (pileObject is DocumentBase document)
        {
            Pile.Documents.Add(document);
        }
        
        ChangesMade = true;
    }

    [MemberNotNull(nameof(Pile))]
    public void RemovePileObject(IPileObject pileObject)
    {
        Pile ??= new Pile();
        
        if (PileObjects.Remove(pileObject))
        {
            if (pileObject is Note note)
            {
                Pile.Notes.Add(note);
            }
            else if (pileObject is DocumentBase document)
            {
                Pile.Documents.Add(document);
            }
        }
        
        ChangesMade = true;
    }

    private async Task<bool> RequestSaveChangesIfUnsaved()
    {
        if (!ChangesMade)
            return true;
        
        var option = await MessageBoxManager.GetMessageBoxStandard(
            "Exit?", "The current pile has unsaved changes. Would you like to save?",
            ButtonEnum.YesNoCancel).ShowAsync();
        
        if (option == ButtonResult.Yes)
        {
            await SavePile();

            return false;
        }

        return true;
    }
}