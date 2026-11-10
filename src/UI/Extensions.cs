using FSNotes.UI.Common;
using FSNotes.UI.Features;
using Microsoft.Extensions.Hosting;

namespace FSNotes.UI;

public static class Extensions
{
    public static TBuilder AddUI<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddCommon();
        builder.AddFeatures();

        return builder;
    }
}
