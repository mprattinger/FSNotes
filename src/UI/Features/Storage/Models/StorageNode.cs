namespace FSNotes.UI.Features.Storage.Models;

public class StorageNode
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public bool IsFile { get; set; }
    public int Level { get; set; }
    public List<StorageNode> Children { get; set; } = [];
}