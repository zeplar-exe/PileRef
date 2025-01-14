using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Model;

namespace PileRef.View;

public abstract partial class ObjectViewBase : UserControl
{
    public abstract IPileObject PileObject { get; }
    
    public static readonly RoutedEvent<PointerPressedEventArgs> RequestSelectEvent =
        RoutedEvent.Register<ObjectViewBase, PointerPressedEventArgs>(nameof(RequestSelect), RoutingStrategies.Direct);
    
    public event EventHandler<RoutedEventArgs> RequestSelect
    {
        add => AddHandler(RequestSelectEvent, value);
        remove => RemoveHandler(RequestSelectEvent, value);
    }
    
    public static readonly RoutedEvent<PointerPressedEventArgs> RequestInteractEvent =
        RoutedEvent.Register<ObjectViewBase, PointerPressedEventArgs>(nameof(RequestInteract), RoutingStrategies.Direct);
    
    public event EventHandler<RoutedEventArgs> RequestInteract
    {
        add => AddHandler(RequestInteractEvent, value);
        remove => RemoveHandler(RequestInteractEvent, value);
    }

    public ObjectViewBase()
    {
        AddHandler(PointerPressedEvent, ControlPressed, RoutingStrategies.Tunnel);
    }

    public abstract void BeginInteract();
    public abstract void EndInteract();
    
    protected void ControlPressed(object? sender, PointerPressedEventArgs e)
    {
        var selectArgs = new PileObjectSelectEventArgs(RequestSelectEvent, e);
        var interactArgs = new PileObjectSelectEventArgs(RequestInteractEvent, e);

        if (e.ClickCount < 2)
        {
            RaiseEvent(selectArgs);
        }
        else
        {
            RaiseEvent(interactArgs);
        }
    }
}