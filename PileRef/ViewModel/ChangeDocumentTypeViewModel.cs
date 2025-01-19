using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using PileRef.Common;
using PileRef.Model.Document;

using SystemEncoding = System.Text.Encoding;

namespace PileRef.ViewModel;

public partial class ChangeDocumentTypeViewModel : ObservableObject
{
    public DocumentType[] DocumentTypes => DocumentTypeEnum.Values;
    public IEnumerable<EncodingInfo> Encodings => SystemEncoding.GetEncodings().Reverse();
    
    [ObservableProperty] public partial DocumentType DocumentType { get; set; }
    [ObservableProperty] public partial EncodingInfo Encoding { get; set; } = SystemEncoding.UTF8.GetInfo();
    public bool IsTextEncodable => DocumentType.Flags.HasFlag(DocumentFlags.TextEncodable);

    partial void OnDocumentTypeChanged(DocumentType value)
    {
        OnPropertyChanged(nameof(IsTextEncodable));
    }
}