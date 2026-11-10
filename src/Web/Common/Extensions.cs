namespace FSNotes.Web.Common;

public static class Extensions
{
    public static TBuilder AddCommon<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        return builder;
    }
}