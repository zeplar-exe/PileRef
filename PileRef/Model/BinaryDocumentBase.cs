using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PileRef.Model;

public partial class BinaryDocumentBase : DocumentBase
{
    [ObservableProperty] private byte[] content;

    public BinaryDocumentBase(byte[] content, DocumentUri uri) : base(uri)
    {
        Content = content;
    }
}