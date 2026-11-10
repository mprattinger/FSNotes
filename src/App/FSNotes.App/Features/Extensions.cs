using Microsoft.Extensions.Hosting;

namespace FSNotes.App.Features;

public static class Extensions
{
    public static TBuilder AddFeatures<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        return builder;
    }
}