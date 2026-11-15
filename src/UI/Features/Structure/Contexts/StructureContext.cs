using FSNotes.UI.Features.Structure.Models;
using FSNotes.UI.Features.Structure.Services;

namespace FSNotes.UI.Features.Structure.Contexts;

public interface IStructureContext
{
    List<StructureEntity> Structures { get; set; }
    event Action? Changed;
    Task Reload();
}

public class StructureContext(IStructureService structureService) : IStructureContext
{
    public List<StructureEntity> Structures { get; set; } = [];

    public event Action? Changed;

    public async Task Reload()
    {
        Structures = await structureService.GetStructureAsync();
        Changed?.Invoke();
    }
}
