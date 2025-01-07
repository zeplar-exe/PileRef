using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using PileRef.JsonConfig;
using Serilog;

namespace PileRef
{
    public partial class App : Application
    {
        public const string LogFileName = "pileref.log";
        
        public static HttpClient HttpClient { get; private set; } = new();
        public static AppSettings Settings { get; } = new();

        public App()
        {
            HttpClient = new HttpClient();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
#if RELEASE 
                .WriteTo.File(LogFileName) 
#endif
                .CreateLogger();
            
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new EncodingConverter() }
            };
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