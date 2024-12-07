using System.Net.Http;
using System.Text;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace PileRef
{
    public partial class App : Application
    {
        public static HttpClient HttpClient { get; private set; } = new();

        public App()
        {
            HttpClient = new HttpClient();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}