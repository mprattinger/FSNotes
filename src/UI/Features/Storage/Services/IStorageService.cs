using FSNotes.UI.Features.Storage.Models;

namespace FSNotes.UI.Features.Storage.Services;

public interface IStorageService
{
    StorageNode RootTree { get; }
    string GetRootPath { get; }

    string GetDraftNote(string parent = "");
    void CreateFolder(string name, string parent = "");
    string GetNote(string note, string path);

    /// <summary>
    /// Creates a new Note
    /// </summary>
    /// <param name="name">Name of the Note</param>
    /// <param name="folder">Folder where the note should be stored. If empty store it in the root</param>
    /// <param name="content">Markdown string with the note content</param>
    /// <returns>Fullpath to the note</returns>
    string CreateNote(string name, string folder = "", string content = "");
    void RenameNote(string note, string newName);
    void MoveNote(string note, string parent);
    void MoveFolder(string folder, string parent);
    void DeleteNote(string note);
    void DeleteFolder(string folder);
    string StoreImage(string? name, MemoryStream image);
    StorageSearchResult? FolderExists(string folder);
    StorageSearchResult? NoteExists(string note);
    IEnumerable<StorageSearchResult> Search(string searchTerm);

    void Refresh();
}
