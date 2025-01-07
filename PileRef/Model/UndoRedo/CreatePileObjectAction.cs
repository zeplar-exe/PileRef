using System;
using System.IO;
using System.Threading.Tasks;
using MsBox.Avalonia;
using PileRef.Model.Document;

namespace PileRef.Model.UndoRedo;

public class CreatePileObjectAction : IUserAction
{
    private Pile Pile { get; }
    private IPileObject PileObject { get; }

    public CreatePileObjectAction(Pile pile, IPileObject pileObject)
    {
        Pile = pile;
        PileObject = pileObject;
    }

    public bool Undo()
    {
        if (PileObject is Note note)
            Pile.Notes.Remove(note);
        else if (PileObject is DocumentBase document)
            Pile.Documents.Remove(document);
        
        return true;
    }

    public bool Redo()
    {
        if (PileObject is Note note)
            Pile.Notes.Add(note);
        else if (PileObject is DocumentBase document)
        {
            if (document.Uri.IsFile && !File.Exists(document.Uri.Path))
            {
                Task.Run(async () =>
                {
                    await MessageBoxManager.GetMessageBoxStandard(
                            "Redo Failed",
                            $"Failed to redo. The file the target document was linked to no longer exists. ({document.Uri.Path})")
                        .ShowAsync();
                });
                
                return false;
            }
            
            Pile.Documents.Add(document);
        }

        return true;
    }
}