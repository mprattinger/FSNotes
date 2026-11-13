using FSNotes.UI.Features.Structure.Models;
using FSNotes.UI.Features.Structure.Services;

namespace FSNotes.Web.Features.Structure.Services;

public class StructureService : IStructureService
{
    /*
         * Folder1
         *  -> SubFolder1
         *    -> SubSubFolder1
         *    -> Note1
         *  -> SubFolder2
         * Folder2
         * Folder3
         */

    private List<StructureEntity> data = [
        new() {
            Id = "01a1d527-f665-7ad5-b016-ec0690a2f997",
                Parent = null,
                Type = StructureType.Folder,
                Name = "Folder1"
        },
        new() {
            Id = "01a1d527-f67c-784b-93aa-444624facd70",
                Parent = null,
                Type = StructureType.Folder,
                Name = "Folder1"
        },
        new() {
            Id = "01a1d527-f67c-7666-94a9-713f90465bd3",
                Parent = null,
                Type = StructureType.Folder,
                Name = "Folder1"
        },
        new() {
            Id = "01a1d52a-4b90-7327-92de-b53ab6c597ba",
                Parent = "01a1d527-f665-7ad5-b016-ec0690a2f997",
                Type = StructureType.Folder,
                Name = "SubFolder1"
        },
        new() {
            Id = "01a1d52a-86e9-773f-9359-0f9e45edaa5d",
                Parent = "01a1d527-f665-7ad5-b016-ec0690a2f997",
                Type = StructureType.Folder,
                Name = "SubFolder2"
        },
        new() {
            Id = "01a1d52a-e52d-7f0d-ae73-14ec5a5520cf",
                Parent = "01a1d52a-4b90-7327-92de-b53ab6c597ba",
                Type = StructureType.Folder,
                Name = "SubSubFolder1"
        },
        new() {
            Id = "01a1d52b-7e59-7728-8d4b-b4a056b8c128",
                Parent = "01a1d52a-4b90-7327-92de-b53ab6c597ba",
                Type = StructureType.Note,
                Name = "Note1"
        },
        ];

    public async Task<List<StructureEntity>> GetStructureAsync()
    {
        return await getStructureAsync();
    }

    public async Task<List<StructureEntity>> AddFolder(string name, string parentId)
    {
        data.Add(new StructureEntity
        {
            Id = Guid.CreateVersion7().ToString(),
            Parent = parentId,
            Type = StructureType.Folder,
            Name = name
        });

        return await getStructureAsync();
    }

    public async Task<List<StructureEntity>> AddNote(string name, string parentId)
    {
        data.Add(new StructureEntity
        {
            Id = Guid.CreateVersion7().ToString(),
            Parent = parentId,
            Type = StructureType.Note,
            Name = name
        });

        return await getStructureAsync();
    }

    public async Task<List<StructureEntity>> RemoveItem(string id, bool recursive = false)
    {
        var entity = data.FirstOrDefault(e => e.Id == id);
        if (entity is null)
        {
            throw new Exception($"Entity with id {id} not found");
        }

        if (recursive)
        {
            var children = data.Where(e => e.Parent == id).ToList();
            foreach (var child in children)
            {
                await RemoveItem(child.Id, true);
            }
        }
        else
        {
            var hasChildren = data.Any(e => e.Parent == id);
            if (hasChildren)
            {
                throw new Exception($"Entity with id {id} has children and cannot be removed without recursive flag");
            }
        }

        return await getStructureAsync();
    }

    private Task<List<StructureEntity>> getStructureAsync()
    {
        var entities = data.ToDictionary(
            entity => entity.Id,
            entity => new StructureEntity
            {
                Id = entity.Id,
                Parent = entity.Parent,
                Type = entity.Type,
                Name = entity.Name
            });
        var ret = new List<StructureEntity>();

        foreach (var entity in data)
        {
            var node = entities[entity.Id];
            if (entity.Parent is not null && entities.TryGetValue(entity.Parent, out var parent))
            {
                node.Level = parent.Level + 1;
                parent.Children.Add(node);
            }
            else
            {
                ret.Add(node);
            }
        }

        return Task.FromResult(ret);
    }

    public async Task<List<StructureEntity>> MoveItem(string id, string newParentId)
    {
        var entity = data.FirstOrDefault(e => e.Id == id);
        if (entity is null)
        {
            throw new Exception($"Entity with id {id} not found");
        }

        entity.Parent = newParentId;

        return await getStructureAsync();
    }

    public async Task<List<StructureEntity>> RenameItem(string id, string newName)
    {
        var entity = data.FirstOrDefault(e => e.Id == id);
        if (entity is null)
        {
            throw new Exception($"Entity with id {id} not found");
        }

        entity.Name = newName;

        return await getStructureAsync();
    }
}