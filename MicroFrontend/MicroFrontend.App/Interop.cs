﻿using Microsoft.JSInterop;

namespace MicroFrontend.App;

public class Interop
{
    private readonly IJSRuntime _jsRuntime;

    public Interop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public Task IncludeLink(string id, string href)
    {
        try
        {
            _jsRuntime.InvokeVoidAsync("MicroFrontendApp.Interop.includeLink", id, href);
            return Task.CompletedTask;
        }
        catch
        {
            return Task.CompletedTask;
        }
    }

    public Task AddLink(string id, string style, string place = "head")
    {
        try
        {
            _jsRuntime.InvokeVoidAsync("MicroFrontendApp.Interop.addLink", id, style, place);
            return Task.CompletedTask;
        }
        catch
        {
            return Task.CompletedTask;
        }
    }

    public Task IncludeScript(string id, string src)
    {
        try
        {
            _jsRuntime.InvokeVoidAsync("MicroFrontendApp.Interop.includeScript", id, src);
            return Task.CompletedTask;
        }
        catch
        {
            return Task.CompletedTask;
        }
    }
}
