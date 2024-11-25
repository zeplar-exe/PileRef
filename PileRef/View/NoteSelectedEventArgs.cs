using System;
using Avalonia.Input;
using Avalonia.Interactivity;
using PileRef.Model;

namespace PileRef;

public class NoteSelectedEventArgs : RoutedEventArgs
{
    public PointerPressedEventArgs PointerPressedArgs { get; }

    public NoteSelectedEventArgs(RoutedEvent e, PointerPressedEventArgs args) : base(e)
    {
        PointerPressedArgs = args;
    }
}