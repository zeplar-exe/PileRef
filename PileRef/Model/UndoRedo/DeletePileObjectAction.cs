using PileRef.Model.Document;

namespace PileRef.Model.UndoRedo;

public class DeletePileObjectAction : IUserAction
{
    private Pile Pile { get; }
    private IPileObject PileObject { get; }

    public DeletePileObjectAction(Pile pile, IPileObject pileObject)
    {
        Pile = pile;
        PileObject = pileObject;
    }

    public bool Undo()
    {
        if (PileObject is Note note)
            Pile.Notes.Add(note);
        else if (PileObject is DocumentBase document)
            Pile.Documents.Add(document);
        
        return true;
    }

    public bool Redo()
    {
        if (PileObject is Note note)
            Pile.Notes.Remove(note);
        else if (PileObject is DocumentBase document)
            Pile.Documents.Remove(document);
        
        return true;
    }
}