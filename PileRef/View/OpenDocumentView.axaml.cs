using System;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
            var dialog = new OpenFileDialog { AllowMultiple = false };
            var file = await dialog.ShowAsync(this);
            
            if (file?.Length > 0)
                ViewModel.FilePath = file[0];
        }

        private void SelectDocument(object? sender, RoutedEventArgs routedEventArgs)
        {
            var fileData = File.ReadAllBytes(ViewModel.FilePath!);
            IDocument? document = null;

            if (string.IsNullOrEmpty(ViewModel.Title))
                ViewModel.Title = "Untitled Document";

            if (ViewModel.DocumentType.IsTextEncodable)
            {
                var text = ViewModel.Encoding.GetString(fileData);
                
                if (ViewModel.DocumentType == DocumentTypeEnum.Markdown) 
                    document = new MarkdownDocument { Title = ViewModel.Title, Content = text };
                else if (ViewModel.DocumentType == DocumentTypeEnum.PlainText) 
                    document = new PlainTextDocument { Title = ViewModel.Title, Content = text };
            }
            else
            {
                
            }

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