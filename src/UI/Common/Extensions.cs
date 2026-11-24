using FSNotes.UI.Common.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FSNotes.UI.Common;

public static class Extensions
{
    public static TBuilder AddCommon<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddScoped<ApplicationContext>();

        return builder;
    }
}