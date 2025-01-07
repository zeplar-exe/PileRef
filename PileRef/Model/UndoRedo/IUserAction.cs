namespace PileRef.Model.UndoRedo;

public interface IUserAction
{
    public bool Undo();
    public bool Redo();
}