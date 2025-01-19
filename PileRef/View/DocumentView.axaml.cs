using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using MsBox.Avalonia;
using PileRef.Model;
using PileRef.Model.Document;
using PileRef.ViewModel;
using DocumentBase = PileRef.Model.Document.DocumentBase;

namespace PileRef.View
{
    public partial class DocumentView : ObjectViewBase
    {
        public DocumentViewModel ViewModel { get; }
        
        public DocumentBase Document
        {
            get => GetValue(DocumentProperty); 
            set => SetValue(DocumentProperty, value); 
        }
        
        public static readonly StyledProperty<DocumentBase> DocumentProperty =
            AvaloniaProperty.Register<DocumentView, DocumentBase>(nameof(Document));
        
        public override IPileObject PileObject => Document;
        
        public DocumentView()
        {
            ViewModel = new DocumentViewModel();
            DataContext = this;
            
            InitializeComponent();
            
            AddHandler(PointerPressedEvent, ControlPressed, RoutingStrategies.Tunnel);
        }

        protected override void OnBeginInteract()
        {
            ViewModel.IsInteracting = true;
        }

        protected override void OnEndInteract()
        {
            ViewModel.IsInteracting = false;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            if (change.Property == DocumentProperty && change.NewValue != null)
            {
                var document = change.GetNewValue<DocumentBase>();
                
                ViewModel.CanNoteConvert = false;
                ViewModel.CanSearchText = false;
            }
        }

        private async void CreateUniqueFile(object? sender, RoutedEventArgs e)
        {
            var storageProvider = TopLevel.GetTopLevel(this)!.StorageProvider;
            
            var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                SuggestedFileName = Path.GetFileName(Document.Uri.Path)
            });
            
            if (file == null)
                return;

            await using (var stream = await file.OpenWriteAsync())
            {
                Document.CopyTo(stream);
            }
            
            Document.Uri = new DocumentUri(Uri.UnescapeDataString(file.Path.AbsolutePath), Document.Uri.IsFile);
        }

        private void OpenSource(object? sender, RoutedEventArgs e)
        {
            var path = Document.Uri.Path;
            
            try
            {
                Process.Start(path);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    path = path.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", path);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", path);
                }
                else
                {
                    throw;
                } // https://stackoverflow.com/a/43232486/16324801
            }
        }

        private async void OpenSourceExplorer(object? sender, RoutedEventArgs e)
        {
            var directory = Path.GetDirectoryName(Document.Uri.Path)!;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    Process.Start("explorer", directory);
                }
                catch (Exception exception)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Open Source",
                        $"Failed to open document source: {exception.GetType()}").ShowAsync();
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("mimeopen", directory);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", $"-R \"{directory}\"");
            }
        }

        private async void ChangeDocumentType(object? sender, RoutedEventArgs e)
        {
            var dialog = new ChangeDocumentTypeDialog(Document);
            var document = await dialog.ShowDialog<DocumentBase?>(this.FindAncestorOfType<Window>()!);
            
            if (document == null)
                return;
            
            
        }

        private void Remove(object? sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RequestMoveEvent, sender));
        }
    }
}