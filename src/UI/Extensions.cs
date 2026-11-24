using Blazorise;
using FSNotes.UI.Common;
using FSNotes.UI.Features;
using Microsoft.Extensions.Hosting;

namespace FSNotes.UI;

public static class Extensions
{
    public static TBuilder AddUI<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddBlazorise(options =>
        {
            options.Immediate = true;
            options.ProductToken = builder.Configuration["Licensing:BlazoriseProductToken"] ?? throw new InvalidOperationException("Blazorise product token is not configured. Please set the 'Licensing:BlazoriseProductToken' configuration value.");
        })
            .AddEmptyProviders();

        builder.AddCommon();
        builder.AddFeatures();

        return builder;
    }
}
