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
    protected bool IsInteracting { get; set; }
    
    public abstract IPileObject PileObject { get; }
    
    public static readonly RoutedEvent<RoutedEventArgs> RequestSelectEvent =
        RoutedEvent.Register<ObjectViewBase, RoutedEventArgs>(nameof(RequestSelect), RoutingStrategies.Direct);
    
    public event EventHandler<RoutedEventArgs> RequestSelect
    {
        add => AddHandler(RequestSelectEvent, value);
        remove => RemoveHandler(RequestSelectEvent, value);
    }
    
    public static readonly RoutedEvent<RoutedEventArgs> RequestInteractEvent =
        RoutedEvent.Register<ObjectViewBase, RoutedEventArgs>(nameof(RequestInteract), RoutingStrategies.Direct);
    
    public event EventHandler<RoutedEventArgs> RequestInteract
    {
        add => AddHandler(RequestInteractEvent, value);
        remove => RemoveHandler(RequestInteractEvent, value);
    }
    
    public static readonly RoutedEvent<RoutedEventArgs> RequestMoveEvent =
        RoutedEvent.Register<ObjectViewBase, RoutedEventArgs>(nameof(RequestMove), RoutingStrategies.Direct);
    
    public event EventHandler<RoutedEventArgs> RequestMove
    {
        add => AddHandler(RequestMoveEvent, value);
        remove => RemoveHandler(RequestMoveEvent, value);
    }
    
    public static readonly RoutedEvent<RoutedEventArgs> RequestDeleteEvent =
        RoutedEvent.Register<ObjectViewBase, RoutedEventArgs>(nameof(RequestDelete), RoutingStrategies.Direct);
    
    public event EventHandler<RoutedEventArgs> RequestDelete
    {
        add => AddHandler(RequestDeleteEvent, value);
        remove => RemoveHandler(RequestDeleteEvent, value);
    }

    public ObjectViewBase()
    {
        AddHandler(PointerPressedEvent, ControlPressed, RoutingStrategies.Tunnel);
    }

    public void BeginInteract()
    {
        IsInteracting = true;
        OnBeginInteract();
    }
    
    protected abstract void OnBeginInteract();

    public void EndInteract()
    {
        IsInteracting = false;
        OnEndInteract();
    }
    protected abstract void OnEndInteract();
    
    protected virtual void ControlPressed(object? sender, PointerPressedEventArgs e)
    {
        if (IsInteracting)
            return;
        
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;
            
        e.Handled = true;
        
        if (e.ClickCount < 2)
        {
            RaiseEvent(new RoutedEventArgs(RequestSelectEvent, this));
        }
        else
        {
            RaiseEvent(new RoutedEventArgs(RequestInteractEvent, this));
        }
    }
}