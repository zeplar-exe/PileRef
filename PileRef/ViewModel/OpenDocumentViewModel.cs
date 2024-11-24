using System;
using System.IO;
using System.Text;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;
using SystemEncoding = System.Text.Encoding;

namespace PileRef.ViewModel;

public partial class OpenDocumentViewModel : ObservableObject
{
    [ObservableProperty] private string? filePath;
    [ObservableProperty] private string? title;
    [ObservableProperty] private Encoding encoding = SystemEncoding.UTF8;
    [ObservableProperty] private DocumentType documentType = DocumentTypeEnum.PlainText;

    public DocumentType[] DocumentTypes => DocumentTypeEnum.Values;
    public EncodingInfo[] Encodings => SystemEncoding.GetEncodings();
    
    public bool TitleEmpty => string.IsNullOrEmpty(Title);
    public bool FileExists => File.Exists(FilePath);
    public bool FormCompleted => !string.IsNullOrEmpty(FilePath) && FileExists;

    partial void OnTitleChanged(string? oldValue, string? newValue)
    {
        OnPropertyChanged(nameof(TitleEmpty));
        OnPropertyChanged(nameof(FormCompleted));
    }

    partial void OnFilePathChanged(string? oldValue, string? newValue)
    {
        OnPropertyChanged(nameof(FormCompleted));
        OnPropertyChanged(nameof(FileExists));

        DocumentType = Path.GetExtension(newValue) switch
        {
            ".txt" => DocumentTypeEnum.PlainText,
            ".md" or ".markdown" => DocumentTypeEnum.Markdown,
            ".bin" or ".dll" or ".bmp" or ".dat" => DocumentTypeEnum.Binary,
            ".hex" => DocumentTypeEnum.Hexadecimal,
            _ => DocumentType
        };
    }
}