using Avalonia;
using Avalonia.Controls;
using PileRef.Model.Document;

namespace PileRef.Model.UndoRedo;

public class MovePileObjectAction : IUserAction
{
    private IPileObject PileObject { get; }
    private Point OldPoint { get; }
    private Point NewPoint { get; }

    public MovePileObjectAction(IPileObject pileObject, Point oldPoint, Point newPoint)
    {
        PileObject = pileObject;
        OldPoint = oldPoint;
        NewPoint = newPoint;
    }

    public bool Undo()
    {
        PileObject.XPosition = OldPoint.X;
        PileObject.YPosition = OldPoint.Y;
        
        return true;
    }

    public bool Redo()
    {
        PileObject.XPosition = NewPoint.X;
        PileObject.YPosition = NewPoint.Y;
        
        return true;
    }
}