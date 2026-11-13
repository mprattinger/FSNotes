using FSNotes.UI.Features.Structure.Models;

namespace FSNotes.UI.Features.Structure.Services;

public interface IStructureService
{
    Task<List<StructureEntity>> GetStructureAsync();

    Task<List<StructureEntity>> AddFolder(string name, string parentId);

    Task<List<StructureEntity>> AddNote(string name, string parentId);

    Task<List<StructureEntity>> RemoveItem(string id, bool recursive = false);

    Task<List<StructureEntity>> MoveItem(string id, string newParentId);

    Task<List<StructureEntity>> RenameItem(string id, string newName);
}