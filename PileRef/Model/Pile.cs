using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PileRef.Model.Document;
using Serilog;

namespace PileRef.Model;

public partial class Pile
{
    public const int FormatVersion = 0;
    
    public List<DocumentBase> Documents { get; private init; } = [];
    public List<Note> Notes { get; private init; } = [];

    public JObject ToJson()
    {
        var notes = Notes.Select(JObject.FromObject);
        var documents = new List<JObject>();

        foreach (var document in Documents)
        {
            var typeEnum = DocumentTypeEnum.GetEnumFromType(document.GetType());

            if (typeEnum == null)
            {
                Log.Logger.Debug($"Discarding document \"{document.Title}\". Document class {document.GetType()} is not supported.");
                
                continue;
            }

            var type = typeEnum.Id;
           
            var documentJson = JObject.FromObject(document);
            documentJson["$type"] = type;
            
            documents.Add(documentJson);
        }
        
        var json = new JObject
        {
            [nameof(FormatVersion)] = FormatVersion,
            [nameof(Notes)] = new JArray(notes),
            [nameof(Documents)] = new JArray(documents)
        };

        return json;
    }

    public static async Task<Pile> FromJsonAsync(JObject json)
    {
        var notes = json[nameof(Notes)]?.ToObject<Note[]>() ?? [];
        var documents = json[nameof(Documents)];

        if (documents?.Type != JTokenType.Array)
            return new Pile { Notes = new List<Note>(notes) };

        var deserializedDocuments = new List<DocumentBase>();

        foreach (var document in documents)
        {
            var type = document["$type"]?.Value<string>();
            
            if (type == null)
                continue;

            var typeEnum = DocumentTypeEnum.Values.SingleOrDefault(e => e.Id == type);

            if (typeEnum == null)
            {
                Log.Logger.Debug($"Discarding document of type {type} from load. Not supported.");
                
                continue;
            }
            
            var uri = document["uri"]?.ToObject<DocumentUri>();

            if (uri == null)
            {
                Log.Logger.Debug($"Discarding document of type {type} from load. Invalid uri.");
                
                continue;
            }
            
            var stream = await uri.OpenAsync();

            if (stream == null)
            {
                Log.Logger.Debug($"Discarding document of type {type} from load. Failed to open stream.");
                
                continue;
            }
            
            var encodingName = document["encoding"]?.Value<string>();
            var encoding = encodingName != null ? Encoding.GetEncoding(encodingName) : Encoding.Default;
            var doc = DocumentTypeEnum.CreateDocumentFromEnum(typeEnum, stream, uri, encoding);

            if (doc == null)
            {
                Log.Logger.Debug($"Discarding document of type {type} from load. Not supported.");
                
                continue;
            }
            
            using (var reader = document.CreateReader())
            {
                JsonSerializer.CreateDefault().Populate(reader, doc);
            }
            
            deserializedDocuments.Add(doc);
        }
        
        return new Pile { 
            Notes = new List<Note>(notes), 
            Documents = new List<DocumentBase>(deserializedDocuments) 
        };
    }
}