using System;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using PileRef.Model.Document;
using PileRef.ViewModel;
using DocumentBase = PileRef.Model.Document.DocumentBase;

namespace PileRef.View
{
    public partial class OpenDocumentView : Window
    {
        public OpenDocumentViewModel ViewModel { get; }
        
        public OpenDocumentView()
        {
            ViewModel = new OpenDocumentViewModel();
            DataContext = ViewModel;
            
            InitializeComponent();
        }

        private async void OpenFile(object? sender, RoutedEventArgs routedEventArgs)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false
            });
            
            if (files.Count > 0)
                ViewModel.Uri = Uri.UnescapeDataString(files[0].Path.AbsolutePath);
        }

        private async void SelectDocument(object? sender, RoutedEventArgs routedEventArgs)
        {
            var uri = new DocumentUri(ViewModel.Uri, ViewModel.UriIsFile);
            var stream = await uri.OpenAsync();
            
            if (stream == null)
                return;

            DocumentBase? document = null;

            if (string.IsNullOrEmpty(ViewModel.Title))
                ViewModel.Title = "Untitled Document";

            document = DocumentTypeEnum.CreateDocumentFromEnum(ViewModel.DocumentType, stream, uri, ViewModel.Encoding);

            if (document == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(
                    "Unsupported Document",
                    $"Document type \"{ViewModel.DocumentType.DisplayName}\" is not implemented.").ShowAsync();
                
                return;
            }
            
            document.Title = ViewModel.Title;

            Close(document);
        }

        private void CancelSelect(object? sender, RoutedEventArgs routedEventArgs)
        {
            Close(null);
        }

        private void DocumentTypeSelected(object? sender, SelectionChangedEventArgs e)
        {
            var type = e.AddedItems[0] as DocumentType;
            
            ViewModel.DocumentType = type!;
        }

        private void EncodingSelected(object? sender, SelectionChangedEventArgs e)
        {
            var info = e.AddedItems[0] as EncodingInfo;

            ViewModel.Encoding = info!.GetEncoding();
        }
    }
}