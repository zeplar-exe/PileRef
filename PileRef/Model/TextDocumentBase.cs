using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public abstract partial class TextDocumentBase : DocumentBase
{
    [ObservableProperty] private string content;
    
    protected TextDocumentBase(string content, DocumentUri uri) : base(uri)
    {
        Content = content;
    }
}