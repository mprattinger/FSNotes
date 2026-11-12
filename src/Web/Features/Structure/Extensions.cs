using FSNotes.UI.Features.Structure.Services;
using FSNotes.Web.Features.Structure.Services;

namespace FSNotes.Web.Features.Structure;

public static class Extensions
{
    public static TBuilder AddStructure<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddScoped<IStructureService, StructureService>();

        return builder;
    }
}