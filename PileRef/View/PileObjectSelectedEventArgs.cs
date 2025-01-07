using Avalonia.Input;
using Avalonia.Interactivity;

namespace PileRef.View;

public class PileObjectSelectedEventArgs : RoutedEventArgs
{
    public PointerEventArgs PointerArgs { get; }

    public PileObjectSelectedEventArgs(RoutedEvent e, PointerEventArgs args) : base(e)
    {
        PointerArgs = args;
    }
}