using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using PileRef.Model;

namespace PileRef;

public class PileObjectTemplateSelector : IDataTemplate
{
    [Content]
    public Dictionary<Type, IDataTemplate> Templates { get; } = new();
    
    public Control Build(object? data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var initialType = data.GetType();
        var target = Templates.GetValueOrDefault(initialType);
        var check = new Queue<Type>();
        check.Enqueue(initialType);

        while (check.Count > 0)
        {
            var type = check.Dequeue();
            target = Templates.GetValueOrDefault(type);

            if (target != null)
                break;
        
            if (type.BaseType != null)
                check.Enqueue(type.BaseType);

            foreach (var i in type.GetInterfaces())
                check.Enqueue(i);
        }
        
        if (target == null)
            throw new ArgumentException($"{data.GetType()} and its ancestors are not bound in PileObjectTemplateSelector.");
        
        return target.Build(data);
    }

    public bool Match(object? data)
    {
        return data != null && data.GetType().IsAssignableTo(typeof(IPileObject));
    }
}