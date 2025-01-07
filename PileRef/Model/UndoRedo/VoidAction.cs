namespace PileRef.Model.UndoRedo;

public class VoidAction : IUserAction
{
    public bool Undo()
    {
        return false;
    }

    public bool Redo()
    {
        return false;
    }
}