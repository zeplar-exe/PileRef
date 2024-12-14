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
using DocumentBase = PileRef.Model.DocumentBase;
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
            var uri = new DocumentUri(ViewModel.Uri, ViewModel.UriIsFile);
            var stream = await uri.OpenAsync();

            DocumentBase? document = null;

            if (string.IsNullOrEmpty(ViewModel.Title))
                ViewModel.Title = "Untitled Document";

            if (ViewModel.DocumentType == DocumentTypeEnum.Markdown) 
                document = new MarkdownDocument(stream, uri, ViewModel.Encoding);
            else if (ViewModel.DocumentType == DocumentTypeEnum.PlainText) 
                document = new PlainTextDocument(stream, uri, ViewModel.Encoding);
            else if (ViewModel.DocumentType == DocumentTypeEnum.Latex)
                document = new LatexDocument(stream, uri, ViewModel.Encoding);
            else if (ViewModel.DocumentType == DocumentTypeEnum.DOC)
                document = new OldWordDocument(stream, uri, ViewModel.Encoding);
            else if (ViewModel.DocumentType == DocumentTypeEnum.ODT)
                document = new OdtDocument(stream, uri);
            else if (ViewModel.DocumentType == DocumentTypeEnum.SVG)
                document = new SvgDocument(stream, uri, ViewModel.Encoding);
            else if (ViewModel.DocumentType == DocumentTypeEnum.PAGES)
                document = new PagesDocument(stream, uri);
            // https://web.archive.org/web/20241120144313/https://www.tempmail.us.com/en/iwork/effortlessly-accessing-pages-and-numbers-files-with-c-on-windows
            else if (ViewModel.DocumentType == DocumentTypeEnum.RTF)
                document = new RichTextDocument(stream, uri, ViewModel.Encoding);
            else if (ViewModel.DocumentType == DocumentTypeEnum.XPS)
                document = new XpsDocument(stream, uri);
            else if (ViewModel.DocumentType == DocumentTypeEnum.PDF)
                document = new PilePdfDocument(stream, uri);
            else if (ViewModel.DocumentType == DocumentTypeEnum.EPUB)
                document = new EpubDocument(stream, uri);
            else if (ViewModel.DocumentType == DocumentTypeEnum.PNG)
                document = new ImageDocument(stream, uri);
            else if (ViewModel.DocumentType == DocumentTypeEnum.JPG)
                document = new ImageDocument(stream, uri);
            else if (ViewModel.DocumentType == DocumentTypeEnum.BMP)
                document = new ImageDocument(stream, uri);
            else if (ViewModel.DocumentType == DocumentTypeEnum.DOCX)
                document = new XmlWordDocument(stream, uri);

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