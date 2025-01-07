using Newtonsoft.Json;

namespace PileRef;

public class LocaleInfo
{
    [JsonProperty("name")]
    public string DisplayName { get; }
    
    [JsonProperty("path")]
    public string ResourceName { get; }
    
    public LocaleInfo(string displayName, string resourceName)
    {
        DisplayName = displayName;
        ResourceName = resourceName;
    }
}