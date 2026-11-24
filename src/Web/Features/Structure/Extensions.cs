namespace FSNotes.Web.Features.Structure;

public static class Extensions
{
    public static TBuilder AddStructure<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {

        return builder;
    }
}