namespace FSNotes.UI.Features.Structure.Models;

public class StructureEntity
{
    public string Id { get; set; }
    public string? Parent { get; set; }
    public StructureType Type { get; set; }
    public string Name { get; set; } = "";

    public int Level { get; set; }

    public List<StructureEntity> Children { get; set; } = [];
}