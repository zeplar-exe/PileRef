using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace PileRef;

[JsonObject(MemberSerialization.OptIn)]
public partial class AppSettings : ObservableObject
{
    private string FilePath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "appsettings.json");

    public ObservableCollection<LocaleInfo> Locales { get; } = [];
    
    [ObservableProperty] [JsonProperty("locale_info")]
    public partial LocaleInfo CurrentLocaleInfo { get; set; }
    
    public Locale CurrentLocale { get; private set; }

    public AppSettings()
    {
        foreach(var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
        {
            if (!resourceName.Contains("locale.") || !resourceName.EndsWith(".json")) 
                continue;
            
            var name = Path.GetFileNameWithoutExtension(resourceName);
                
            Locales.Add(new LocaleInfo(name, resourceName));
        }

        var targetLocale = Locales.SingleOrDefault(l => l?.DisplayName == "English", Locales.FirstOrDefault());
        
        if (targetLocale == null)
            Log.Warning($"No locales available on startup.");
        
        targetLocale ??= new LocaleInfo("null", "null");
        
        CurrentLocaleInfo = targetLocale;
    }

    public async void Save()
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        
        await File.WriteAllTextAsync(FilePath, json);
    }

    public async void Load()
    {
        var json = JObject.Parse(await File.ReadAllTextAsync(FilePath));

        var localeInfo = json["locale"]?.ToObject<LocaleInfo>();
        
        if (localeInfo != null)
            CurrentLocaleInfo = localeInfo;
    }

    partial void OnCurrentLocaleInfoChanged(LocaleInfo oldValue, LocaleInfo newValue)
    {
        CurrentLocale = new Locale(newValue);
    }
}