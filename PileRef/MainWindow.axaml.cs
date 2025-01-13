using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PileRef.Model;
using PileRef.Model.Document;
using PileRef.View;
using PileRef.ViewModel;
using Serilog;

namespace PileRef
{
    public partial class MainWindow : Window
    {
        private List<IPileObject> SelectedObjects { get; } = [];
        private bool IsMouseDown { get; set; }
        private bool IsDragMouseDown { get; set; }
        private bool WasDragging { get; set; }
        private Point LastMousePosition { get; set; }
        
        public MainWindowViewModel ViewModel { get; }
        
        public static float Dpi { get; set; }
        
        public MainWindow()
        {
            #if DEBUG
            this.AttachDevTools();
            #endif
            
            ViewModel = new MainWindowViewModel(StorageProvider);
            DataContext = ViewModel;
            
            Dpi = (float)(96f * RenderScaling);
            
            InitializeComponent();
            
            AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);
            AddHandler(KeyUpEvent, OnKeyUp, RoutingStrategies.Tunnel, handledEventsToo: true);
            AddHandler(PointerPressedEvent, OnMouseDown, RoutingStrategies.Tunnel, handledEventsToo: true);
            AddHandler(PointerPressedEvent, OnMouseDownBubble, RoutingStrategies.Bubble, handledEventsToo: true);
            AddHandler(PointerMovedEvent, OnMouseMove, RoutingStrategies.Tunnel, handledEventsToo: true);
            AddHandler(PointerReleasedEvent, OnMouseUp, RoutingStrategies.Tunnel, handledEventsToo: true);
            AddHandler(PointerReleasedEvent, OnMouseUpBubble, RoutingStrategies.Bubble, handledEventsToo: true);
            AddHandler(DragDrop.DropEvent, OnDrop);
        }

        private void OnCreateNoteReleased(object? sender, PointerReleasedEventArgs e)
        {
            var menu = (sender as MenuItem)!;
            var pos = v_PileView.PointToClient(menu.PointToScreen(new Point(0, 0)));

            ViewModel.CreateNote(pos);
        }
        
        private void OnOpenDocumentReleased(object? sender, PointerReleasedEventArgs e)
        {
            var menu = (sender as MenuItem)!;
            var pos = v_PileView.PointToClient(menu.PointToScreen(new Point(0, 0)));

            ViewModel.OpenDocument(pos, this);
        }

        private void OnNoteSelectedDown(object? sender, RoutedEventArgs e)
        {
            if (sender is not NoteView view)
                return;
            
            HandlePileObjectSelect(view.Note, (PileObjectSelectedEventArgs)e, false);
        }
        
        private void OnNoteSelectedUp(object? sender, RoutedEventArgs e)
        {
            if (sender is not NoteView view)
                return;

            HandlePileObjectSelect(view.Note, (PileObjectSelectedEventArgs)e, true);
        }
        
        private void OnDocumentSelected(object? sender, RoutedEventArgs e)
        {
            if (sender is not DocumentView view)
                return;

            var args = (PileObjectSelectedEventArgs)e;
            
            HandlePileObjectSelect(view.Document, args, false);
        }

        private void HandlePileObjectSelect(IPileObject pileObject, PileObjectSelectedEventArgs args, bool isReleasing)
        {
            IsDragMouseDown = !isReleasing;
            
            var pointerArgs = args.PointerArgs;
            
            if (SelectedObjects.Contains(pileObject))
            {
                SelectedObjects.Remove(pileObject);
            }
            else
            {
                SelectedObjects.Clear();
                SelectedObjects.Add(pileObject);
            }
        }

        private void OnWindowGainFocus(object? sender, RoutedEventArgs e)
        {
            SelectedObjects.Clear();
        }

        private void OnMouseDown(object? sender, PointerPressedEventArgs e)
        {
            IsMouseDown = true;
            WasDragging = false;
        }
        
        private void OnMouseDownBubble(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.IsPanning = true;
            SelectedObjects.Clear();
        }
        
        private void OnMouseMove(object? sender, PointerEventArgs e)
        {
            var last = LastMousePosition;
            LastMousePosition = e.GetPosition(this);

            var delta = e.GetPosition(this) - last;
            
            if (!IsMouseDown)
                return;
            
            if (ViewModel.IsPanning)
            {
                ViewModel.PanX += delta.X * ViewModel.DragScale;
                ViewModel.PanY += delta.Y * ViewModel.DragScale;
                
                return;
            }

            WasDragging = true;

            if (IsDragMouseDown)
            {
                foreach (var selected in SelectedObjects)
                {
                    selected.XPosition += delta.X * ViewModel.DragScale;
                    selected.YPosition += delta.Y * ViewModel.DragScale;
                }
            }

            LastMousePosition = e.GetPosition(this);
        }
        
        private void OnMouseUp(object? sender, PointerReleasedEventArgs e)
        {
            IsMouseDown = false;
        }
        
        private void OnMouseUpBubble(object? sender, PointerReleasedEventArgs e)
        {
            ViewModel.IsPanning = false;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ViewModel.IsPanning = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ViewModel.IsPanning = false;
            }
        }

        private void OnWheel(object? sender, PointerWheelEventArgs e)
        {
            ViewModel.ZoomLevel += e.Delta.Y;
        }

        private async void OnDrop(object? sender, DragEventArgs e)
        {
            var pos = e.GetPosition(v_PileView);
            
            string uri;
            bool isFile;
            
            if (e.Data.Contains("UniformResourceLocator"))
            {
                var uriBytes = (byte[])e.Data.Get("UniformResourceLocator")!;
                uri = Encoding.ASCII.GetString(uriBytes).TrimEnd('\0');
                isFile = false;
            }
            else if (e.Data.Contains("FileName"))
            {
                var nameBytes = (byte[])e.Data.Get("FileName")!;
                uri = Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');
                isFile = true;
            }
            else if (e.Data.Contains("Text"))
            {
                var text = e.Data.Get(DataFormats.Text) as string ?? string.Empty;
                var note = new Note { Text = text, XPosition = pos.X, YPosition = pos.Y };
                
                ViewModel.AddPileObject(note);
                
                return;
            }
            else
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Drag & Drop Import", "Unknown entity.")
                    .ShowAsync();
                
                Log.Logger.Debug($"Unknown drag and drop entity: {string.Join(", ", e.Data.GetDataFormats())}");
                
                return;
            }
            
            var dialog = new OpenDocumentView();
            dialog.ViewModel.Uri = uri;
            dialog.ViewModel.UriIsFile = isFile;

            var document = await dialog.ShowDialog<DocumentBase?>(this);
            
            if (document == null)
                return;
            
            document.XPosition = pos.X;
            document.YPosition = pos.Y;
            
            ViewModel.AddPileObject(document);
        }
    }
}