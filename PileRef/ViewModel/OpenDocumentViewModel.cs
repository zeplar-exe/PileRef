using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;
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
        
        var extension = Path.GetExtension(newValue);
        
        foreach (var type in DocumentTypes)
        {
            if (type.Extensions.Contains(extension))
            {
                DocumentType = type;
                
                break;
            }
        }

        if (newValue.StartsWith("http://") || newValue.StartsWith("https://"))
        {
            UriIsFile = false;
        }
    }
}