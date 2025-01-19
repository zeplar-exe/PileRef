using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.Document;
using PileRef.Common;
using PileRef.Model.Document;
using PileRef.ViewModel;

namespace PileRef.View;

public partial class ChangeDocumentTypeDialog : Window
{
    public DocumentBase Document { get; }
    private DocumentType OriginalDocumentType { get; set; }
    
    public ChangeDocumentTypeViewModel ViewModel { get; }
    
    public ChangeDocumentTypeDialog(DocumentBase document)
    {
        Document = document;
        ViewModel = new ChangeDocumentTypeViewModel();
        DataContext = ViewModel;
        
        OriginalDocumentType = DocumentTypeEnum.GetEnumFromType(document.GetType());
        ViewModel.DocumentType = OriginalDocumentType;
        
        if (Document is TextDocumentBase textDocument)
            ViewModel.Encoding = textDocument.Encoding.GetInfo();
        
        InitializeComponent();
    }

    private void Cancel(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }

    private void OnDone(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine(ViewModel.DocumentType);
        Console.WriteLine(ViewModel.Encoding);
        
        var document = DocumentTypeEnum.CreateDocumentFromEnum(ViewModel.DocumentType, Document.Uri, ViewModel.Encoding.GetEncoding());
        
        Close(document);
    }
}