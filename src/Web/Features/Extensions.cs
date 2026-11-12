using FSNotes.Web.Features.Structure;

namespace FSNotes.Web.Features;

public static class Extensions
{
    public static TBuilder AddFeatures<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddStructure();

        return builder;
    }
}