using System;
using Avalonia.Input;
using Avalonia.Interactivity;
using PileRef.Model;

namespace PileRef;

public class PileObjectSelectedEventArgs : RoutedEventArgs
{
    public PointerEventArgs PointerArgs { get; }

    public PileObjectSelectedEventArgs(RoutedEvent e, PointerEventArgs args) : base(e)
    {
        PointerArgs = args;
    }
}