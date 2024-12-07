using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Xilium.CefGlue.Avalonia;

namespace PileRef.View.Document;

public partial class PdfDocumentView : UserControl
{
    private AvaloniaCefBrowser Browser { get; }

    public static readonly StyledProperty<string> AddressProperty = AvaloniaProperty.Register<PdfDocumentView, string>(
        nameof(Address));

    public string Address
    {
        get => GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }
    
    public PdfDocumentView()
    {
        InitializeComponent();

        Browser = new AvaloniaCefBrowser
        {
            
        };
        
        Content = Browser;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
        if (change.Property != AddressProperty)
            return;
        
        Browser.Address = change.NewValue?.ToString() ?? string.Empty;
    }
}