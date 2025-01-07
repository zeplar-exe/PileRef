using System;
using System.Collections.Generic;

namespace PileRef.Model.UndoRedo;

public class ActionManager
{
    private LinkedList<IUserAction> Actions { get; }
    private LinkedListNode<IUserAction> CurrentAction { get; set; }
    
    public bool CanUndo => CurrentAction.Value is not VoidAction;
    public bool CanRedo => CurrentAction.Previous is not null;

    public ActionManager()
    {
        Actions = new LinkedList<IUserAction>();
        Actions.AddFirst(new VoidAction());
        CurrentAction = Actions.First!;
    }
    
    public void AddAction(IUserAction action)
    {
        CurrentAction = Actions.AddFirst(action);
    }

    public void Undo()
    {
        if (CurrentAction.Value.Undo())
            CurrentAction = CurrentAction.Next!;
    }

    public void Redo()
    {
        if (CurrentAction.Previous == null) 
            return;
        
        if (CurrentAction.Previous.Value.Redo())
            CurrentAction = CurrentAction.Previous!;
    }
}