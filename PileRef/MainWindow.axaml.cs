using System;
using System.Collections.Generic;
using System.IO;
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
using PileRef.ViewModel;

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
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            
            Dpi = (float)(96f * RenderScaling);
            
            InitializeComponent();
            
            this.AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);
            this.AddHandler(KeyUpEvent, OnKeyUp, RoutingStrategies.Tunnel, handledEventsToo: true);
            this.AddHandler(PointerPressedEvent, OnMouseDown, RoutingStrategies.Tunnel, handledEventsToo: true);
            this.AddHandler(PointerPressedEvent, OnMouseDownBubble, RoutingStrategies.Bubble, handledEventsToo: true);
            this.AddHandler(PointerMovedEvent, OnMouseMove, RoutingStrategies.Tunnel, handledEventsToo: true);
            this.AddHandler(PointerReleasedEvent, OnMouseUp, RoutingStrategies.Tunnel, handledEventsToo: true);
            this.AddHandler(PointerReleasedEvent, OnMouseUpBubble, RoutingStrategies.Bubble, handledEventsToo: true);
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
        
        private void OnSaveReleased(object? sender, PointerReleasedEventArgs e)
        {
            ViewModel.SavePile(StorageProvider);
        }

        private async void OnCreatePileReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (ViewModel.ChangesMade)
            {
                var result = await MessageBoxManager.GetMessageBoxStandard(
                    "Create New Pile", "The current pile has unsaved changes. Would you like to save?",
                    ButtonEnum.YesNoCancel).ShowAsync();

                if (result == ButtonResult.Yes)
                {
                    ViewModel.SavePile(StorageProvider);
                }
                else if (result != ButtonResult.No)
                {
                    return;
                }
            }
            
            ViewModel.CreatePile();
        }

        private async void OnOpenPileReleased(object? sender, PointerReleasedEventArgs e)
        {
            ViewModel.OpenPile(StorageProvider);
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
    }
}