using FSNotes.UI.Features.Structure.Models;

namespace FSNotes.UI.Features.Structure.Services;

public interface IStructureService
{
    Task<List<StructureEntity>> GetStructureAsync();
}