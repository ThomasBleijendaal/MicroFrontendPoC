﻿namespace MicroFrontend.App.Routing;

internal sealed class RouteContext
{
    public RouteContext(string path)
    {
        // This is a simplification. We are assuming there are no paths like /a//b/. A proper routing
        // implementation would be more sophisticated.
        Segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        // Individual segments are URL-decoded in order to support arbitrary characters, assuming UTF-8 encoding.
        for (int i = 0; i < Segments.Length; i++)
        {
            Segments[i] = Uri.UnescapeDataString(Segments[i]);
        }
    }

    public string[] Segments { get; }

    public Type? Handler { get; set; }

    public IDictionary<string, object>? Parameters { get; set; }
}
