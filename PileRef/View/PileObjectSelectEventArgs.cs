using System;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace PileRef.View;

public class PileObjectSelectEventArgs : RoutedEventArgs
{
    public PointerEventArgs PointerArgs { get; }

    public PileObjectSelectEventArgs(RoutedEvent e, PointerEventArgs args) : base(e)
    {
        PointerArgs = args;
    }
}