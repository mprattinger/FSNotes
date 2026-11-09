using System;
using Microsoft.Extensions.Hosting;

namespace FSNotes.UI;

public static class Extensions
{
    public static TBuilder AddUI<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        return builder;
    }
}
