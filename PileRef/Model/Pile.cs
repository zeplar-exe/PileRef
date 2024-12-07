using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PileRef.Model;

public partial class Pile : ObservableObject
{
    public ObservableCollection<IDocument> Documents { get; private init; } = [];
    public ObservableCollection<Note> Notes { get; private init; } = [];
    
    public IEnumerable<IPileObject> AllObjects => Documents.Concat<IPileObject>(Notes);

    public Pile()
    {
        Documents.CollectionChanged += (sender, args) =>
        {
            OnPropertyChanged(nameof(AllObjects));
        };

        Notes.CollectionChanged += (sender, args) =>
        {
            OnPropertyChanged(nameof(AllObjects));
        };
    }

    public JObject ToJson()
    {
        return new JObject();
    }

    public static Pile FromJson(JObject json)
    {
        var notes = json["notes"]?.ToObject<Note[]>() ?? [];
        var documents = json["documents"];

        if (documents?.Type != JTokenType.Array)
            return new Pile { Notes = new ObservableCollection<Note>(notes) };

        var deserializedDocuments = new List<IDocument>();

        foreach (var document in documents)
        {
            var type = document["type"]?.Value<string>();
            
            if (type == null)
                continue;

            IDocument deserialized;

            if (type == DocumentTypeEnum.Markdown.Name)
                deserialized = document.ToObject<MarkdownDocument>()!;
            else if (type == DocumentTypeEnum.PlainText.Name)
                deserialized = document.ToObject<PlainTextDocument>()!;
            else if (type == DocumentTypeEnum.PDF.Name)
                deserialized = document.ToObject<PilePdfDocument>()!;
            else
                continue;

            deserializedDocuments.Add(deserialized);
        }
        
        return new Pile { 
            Notes = new ObservableCollection<Note>(notes), 
            Documents = new ObservableCollection<IDocument>(deserializedDocuments) 
        };
    }
}