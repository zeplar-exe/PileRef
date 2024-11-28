using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace PileRef;

public abstract class ObjectViewBase<T> : UserControl
{
    public static readonly RoutedEvent<PointerPressedEventArgs> SelectedDownEvent =
        RoutedEvent.Register<T, PointerPressedEventArgs>(nameof(SelectedDown), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> SelectedDown
    {
        add => AddHandler(SelectedDownEvent, value);
        remove => RemoveHandler(SelectedDownEvent, value);
    }
        
    public static readonly RoutedEvent<PointerPressedEventArgs> SelectedUpEvent =
        RoutedEvent.Register<T, PointerPressedEventArgs>(nameof(SelectedUp), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> SelectedUp
    {
        add => AddHandler(SelectedUpEvent, value);
        remove => RemoveHandler(SelectedUpEvent, value);
    }

    public ObjectViewBase()
    {
        this.AddHandler(PointerPressedEvent, ControlPressed, RoutingStrategies.Tunnel);
        this.AddHandler(PointerReleasedEvent, ControlReleased, RoutingStrategies.Tunnel);
    }
    
    protected void ControlPressed(object? sender, PointerPressedEventArgs e)
    {
        var args = new PileObjectSelectedEventArgs(SelectedDownEvent, e);
            
        RaiseEvent(args);
    }
        
    protected void ControlReleased(object? sender, PointerReleasedEventArgs e)
    {
        var args = new PileObjectSelectedEventArgs(SelectedDownEvent, e);
            
        RaiseEvent(args);
    }
}