using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;
using PileRef.Model.Document;
using SystemEncoding = System.Text.Encoding;

namespace PileRef.ViewModel;

public partial class OpenDocumentViewModel : ObservableObject
{
    [ObservableProperty] private string uri = string.Empty;
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private Encoding encoding = SystemEncoding.UTF8;
    [ObservableProperty] private DocumentType documentType = DocumentTypeEnum.PlainText;

    public DocumentType[] DocumentTypes => DocumentTypeEnum.Values;
    public IEnumerable<EncodingInfo> Encodings => SystemEncoding.GetEncodings().Reverse();
    
    [ObservableProperty] private bool uriIsFile = true;
    
    public bool TitleEmpty => string.IsNullOrEmpty(Title);
    public bool FileExists => (UriIsFile && File.Exists(Uri)) || !UriIsFile;
    public bool FormCompleted => !string.IsNullOrEmpty(Uri) && FileExists;

    partial void OnTitleChanged(string? oldValue, string? newValue)
    {
        OnPropertyChanged(nameof(TitleEmpty));
        OnPropertyChanged(nameof(FormCompleted));
    }

    partial void OnUriChanged(string? oldValue, string? newValue)
    {
        OnPropertyChanged(nameof(FormCompleted));
        OnPropertyChanged(nameof(FileExists));
        
        if (newValue == null)
            return;

        var filename = newValue.Split("?").First().Split("/").Last();

        if (filename.Contains("."))
        {
            var extension = filename.Substring(filename.LastIndexOf(".", StringComparison.Ordinal));

            foreach (var type in DocumentTypes)
            {
                if (type.MatchExtension(extension))
                {
                    DocumentType = type;

                    break;
                }
            }
        }

        if (newValue.StartsWith("http://") || newValue.StartsWith("https://"))
        {
            UriIsFile = false;
        }
        
        OnPropertyChanged(nameof(FormCompleted));
        OnPropertyChanged(nameof(FileExists));
    }

    partial void OnUriIsFileChanged(bool oldValue, bool newValue)
    {
        OnPropertyChanged(nameof(FormCompleted));
        OnPropertyChanged(nameof(FileExists));
    }
}