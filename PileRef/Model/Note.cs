using System;
using Avalonia;
using ColorDocument.Avalonia.DocumentElements;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PileRef.Model;

public partial class Note : ObservableObject, IPileObject
{
    [ObservableProperty] [JsonProperty("title")]
    public partial string Title { get; set; } = "Untitled Note";

    [ObservableProperty] [JsonProperty("text")]
    public partial string Text { get; set; }

    [ObservableProperty] [JsonProperty("x")]
    public partial double XPosition { get; set; }

    [ObservableProperty] [JsonProperty("y")]
    public partial double YPosition { get; set; }

    [ObservableProperty]
    [JsonProperty("width")]
    public partial double Width { get; set; } = 280;

    [ObservableProperty]
    [JsonProperty("height")]
    public partial double Height { get; set; } = 300;

    [ObservableProperty] public partial bool ShowTitle { get; set; } = true;
}