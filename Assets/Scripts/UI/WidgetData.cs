using System.Collections.Generic;
using UnityEngine.UIElements;

public struct WidgetData
{
    public static Dictionary<Widgets, VisualElement> WidgetDictionary = new();

    public WidgetData(List<VisualElement> widgets)
    {
        if (WidgetDictionary.Count == 0)
            widgets.ForEach(widget => WidgetDictionary.Add((Widgets)widgets.IndexOf(widget), widget));
    }

    public enum Widgets
    {
        MainMenu,
        OptionsMenu,
        Gauges,
        EndScreen
    }
}
