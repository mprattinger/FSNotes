using FSNotes.UI.Features.Structure.Models;
using FSNotes.UI.Features.Structure.Services;

namespace FSNotes.Web.Features.Structure.Services;

public class StructureService : IStructureService
{
    public Task<List<StructureEntity>> GetStructureAsync()
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

        var ret = new List<StructureEntity>
        {
            new() {
                Id = Guid.NewGuid(),
                Parent = null,
                Type = StructureType.Folder,
                Name = "Folder1",
                Level = 0,
                Children =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Parent = null,
                        Type = StructureType.Folder,
                        Name = "SubFolder1",
                        Level = 1,
                        Children = new List<StructureEntity>
                        {
                            new()
                            {
                                Id = Guid.NewGuid(),
                                Parent = null,
                                Type = StructureType.Folder,
                                Level = 2,
                                Name = "SubSubFolder1"
                            },
                            new()
                            {
                                Id = Guid.NewGuid(),
                                Parent = null,
                                Type = StructureType.Note,
                                Level = 2,
                                Name = "Note1"
                            }
                        }
                    },
                    new()   {
                        Id = Guid.NewGuid(),
                        Parent = null,
                        Type = StructureType.Folder,
                        Level = 1,
                        Name = "SubFolder2"
                    }
                ]
            },
            new() {
                Id = Guid.NewGuid(),
                Parent = null,
                Type = StructureType.Folder,
                Name = "Folder2",
                Level = 0,
            },
            new() {
                Id = Guid.NewGuid(),
                Parent = null,
                Type = StructureType.Folder,
                Name = "Folder3",
                Level = 0,
            },
        };

        return Task.FromResult(ret);
    }
}