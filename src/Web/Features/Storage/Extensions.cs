using FSNotes.UI.Features.Storage.Services;
using FSNotes.Web.Features.Storage.Services;

namespace FSNotes.Web.Features.Storage;

public static class Extensions
{
    public static TBuilder AddStorage<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddScoped<IStorageService, StorageService>();
        return builder;
    }
}