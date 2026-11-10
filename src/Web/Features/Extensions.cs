namespace FSNotes.Web.Features;

public static class Extensions
{
    public static TBuilder AddFeatures<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        return builder;
    }
}