using System;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using PileRef.Model;
using PileRef.ViewModel;
using Encoding = System.Text.Encoding;

namespace PileRef
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
            var uri = new DocumentUri(new Uri(ViewModel.Uri), ViewModel.UriIsFile);
            byte[] bytes;
            
            if (uri.IsFile)
            {
                bytes = await File.ReadAllBytesAsync(ViewModel.Uri);
            }
            else
            {
                bytes = await App.HttpClient.GetByteArrayAsync(ViewModel.Uri);
            }

            IDocument? document = null;

            if (string.IsNullOrEmpty(ViewModel.Title))
                ViewModel.Title = "Untitled Document";

            if (ViewModel.DocumentType.Flags.HasFlag(DocumentFlags.TextEncodable))
            {
                var text = ViewModel.Encoding.GetString(bytes);
                
                if (ViewModel.DocumentType == DocumentTypeEnum.Markdown) 
                    document = new MarkdownDocument(text, uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.PlainText) 
                    document = new PlainTextDocument(text, uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.Latex)
                    document = new LatexDocument(text, uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.Html)
                    document = new HtmlDocument(text, uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.DOC)
                    document = new DocDocument(uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.DOCX)
                    document = new DocxDocument(uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.ODT)
                    document = new OdtDocument(uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.SVG)
                    document = new SvgDocument(text, uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.PAGES)
                    document = new PagesDocument(uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.RTF)
                    document = new RichTextDocument(text, uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.XPS)
                    document = new XpsDocument(uri);
            }
            else
            {
                if (ViewModel.DocumentType == DocumentTypeEnum.PDF)
                    document = new PilePdfDocument(uri);
                else if (ViewModel.DocumentType == DocumentTypeEnum.EPUB)
                    document = new EpubDocument(bytes, uri);
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