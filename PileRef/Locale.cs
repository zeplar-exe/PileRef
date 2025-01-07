using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace PileRef;

public class Locale
{
    public LocaleInfo Info { get; }
    private Dictionary<string, string> Data { get; }

    public Locale(LocaleInfo info)
    {
        Info = info;
        
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(Info.ResourceName);

        if (stream == null)
        {
            Data = new Dictionary<string, string>();
            
            return;
        }
        
        using var reader = new StreamReader(stream);
        
        var json = JObject.Parse(reader.ReadToEnd());
        Data = json.ToObject<Dictionary<string, string>>() ?? new Dictionary<string, string>();
    }

    public string Get(string key)
    {
        return Data.GetValueOrDefault(key, key);
    }
}