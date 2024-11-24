using System.Text;
using Avalonia;

namespace PileRef.Model;

public interface IDocument : IPileObject
{
    public string Title { get; set; }
    public string FilePath { get; set; }
}