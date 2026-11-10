using Microsoft.Extensions.Hosting;

namespace FSNotes.UI.Features;

public static class Extensions
{
    public static TBuilder AddFeatures<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        return builder;
    }
}