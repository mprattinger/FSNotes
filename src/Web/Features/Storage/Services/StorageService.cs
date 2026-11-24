using FSNotes.UI.Features.Storage.Models;
using FSNotes.UI.Features.Storage.Services;

namespace FSNotes.Web.Features.Storage.Services;

public class StorageService : IStorageService
{
    const string DOCROOT = "notes_root";
    const string ASSETROOT = "assets";
    const string NOTESROOT = "notes";
    const string IMAGESROOT = "images";

    private readonly string _docRoot;
    private readonly string _imageRoot;

    private readonly Dictionary<string, List<StorageSearchResult>> _flatIndex = [with(StringComparer.OrdinalIgnoreCase)];

    public StorageNode RootTree { get; private set; } = new();

    string IStorageService.GetRootPath => throw new NotImplementedException();

    public string GetRootPath() => _docRoot;

    public StorageService(IWebHostEnvironment env)
    {
        _docRoot = Path.Combine(env.WebRootPath, DOCROOT, NOTESROOT);
        _imageRoot = Path.Combine(env.WebRootPath, DOCROOT, ASSETROOT, IMAGESROOT);

        Refresh();
    }

    public void CreateFolder(string name, string parent = "")
    {
        //If the folder exists and has the same parent, do nothing
        var folder = FolderExists(name);
        if (folder is not null)
        {
            var folderInfo = new DirectoryInfo(folder.FullPath);
            if (folderInfo.Parent?.FullName == parent)
            {
                return;
            }
        }

        //If we are still here, we need to create the folder
        var folderPath = Path.Combine(parent, name);
        Directory.CreateDirectory(folderPath);
    }

    public void MoveFolder(string folder, string parent)
    {
        var existingFolder = FolderExists(folder) ?? throw new InvalidOperationException($"Folder '{folder}' does not exist.");

        var folderInfo = new DirectoryInfo(existingFolder.FullPath);

        if (existingFolder is not null)
        {
            if (folderInfo.Parent?.FullName == parent)
            {
                //No need to move, already in the correct parent
                return;
            }
        }

        var newFolderPath = Path.Combine(parent, folder);
        folderInfo.MoveTo(newFolderPath);
    }

    public void DeleteFolder(string folder)
    {
        var existingFolder = FolderExists(folder) ?? throw new InvalidOperationException($"Folder '{folder}' does not exist.");

        var folderInfo = new DirectoryInfo(existingFolder.FullPath);
        folderInfo.Delete(true);
    }

    public StorageSearchResult? FolderExists(string folder)
    {
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException("Folder name cannot be empty.", nameof(folder));

        if (string.IsNullOrWhiteSpace(folder))
            return null;
        var searchResults = Search(folder);
        return searchResults.FirstOrDefault(result => !result.IsFile);
    }

    public string CreateNote(string name, string folder = "", string content = "")
    {
        if (string.IsNullOrWhiteSpace(folder))
        {
            folder = _docRoot;
        }

        //Is the note already existing?
        var existingNote = NoteExists(name);
        if (existingNote is not null)
        {
            //also the sme parent folder?
            var fileinfo = new FileInfo(existingNote.FullPath);
            if (fileinfo.DirectoryName == folder)
            {
                //Overwrite the existing note
                File.WriteAllText(existingNote.FullPath, content);
                return existingNote.FullPath;
            }
        }

        var notePath = Path.Combine(folder, $"{name}.md");
        File.WriteAllText(notePath, content);

        return notePath;
    }

    public string GetNote(string note, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            path = _docRoot;
        }

        var notePath = Path.Combine(path, note);

        return File.ReadAllText(notePath);
    }

    public void RenameNote(string note, string newName)
    {
        var existingNote = NoteExists(note) ?? throw new InvalidOperationException($"Note '{note}' does not exist.");

        var fileInfo = new FileInfo(existingNote.FullPath);
        var newNotePath = Path.Combine(fileInfo.DirectoryName ?? _docRoot, $"{newName}.md");

        fileInfo.MoveTo(newNotePath);
    }

    public void MoveNote(string note, string parent)
    {
        var existingNote = NoteExists(note) ?? throw new InvalidOperationException($"Note '{note}' does not exist.");

        var fileInfo = new FileInfo(existingNote.FullPath);
        if (fileInfo.DirectoryName == parent)
        {
            //No need to move, already in the correct parent
            return;
        }

        var newNotePath = Path.Combine(parent, fileInfo.Name);
        fileInfo.MoveTo(newNotePath);
    }

    public void DeleteNote(string note)
    {
        var existingNote = NoteExists(note) ?? throw new InvalidOperationException($"Note '{note}' does not exist.");

        var fileInfo = new FileInfo(existingNote.FullPath);

        //For the future, we need to delete all linked images too

        fileInfo.Delete();
    }

    public string StoreImage(string? name, MemoryStream image)
    {
        var imageName = name ?? $"image_{DateTime.UtcNow:d}_{DateTime.UtcNow:t}";

        var imagePath = Path.Combine(_imageRoot, imageName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
        {
            image.WriteTo(fileStream);
        }

        return imagePath;
    }

    public StorageSearchResult? NoteExists(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new ArgumentException("Note name cannot be empty.", nameof(note));
        var searchResults = Search(note);
        return searchResults.FirstOrDefault(result => result.IsFile);
    }

    public string GetDraftNote(string parent = "")
    {
        throw new NotImplementedException();
    }

    public IEnumerable<StorageSearchResult> Search(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return [];

        searchTerm = searchTerm.Trim();

        // 1. Exact match lookup (Blazing fast via Dictionary)
        if (_flatIndex.TryGetValue(searchTerm, out var exactMatches))
        {
            return exactMatches;
        }

        // 2. Fallback to substring search if exact match isn't found
        return _flatIndex
            .Where(kvp => kvp.Key.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .SelectMany(kvp => kvp.Value);
    }

    public void Refresh()
    {
        _flatIndex.Clear();

        if (Directory.Exists(_docRoot))
        {
            var rootDirInfo = new DirectoryInfo(_docRoot);
            RootTree = traverseDirectory(rootDirInfo, -1);
        }
        else
        {
            RootTree = new StorageNode { Name = "docroot", FullPath = _docRoot, IsFile = false, Level = 0 };
        }
    }

    private StorageNode traverseDirectory(DirectoryInfo dirInfo, int level)
    {
        var newLevel = level + 1;
        var folderNode = new StorageNode
        {
            Name = dirInfo.Name,
            FullPath = dirInfo.FullName,
            IsFile = false,
            Level = newLevel
        };

        // Add folder to the flattened search index
        addToIndex(folderNode.Name, folderNode.FullPath, isFile: folderNode.IsFile);

        // Recursively process subdirectories
        foreach (var subDir in dirInfo.GetDirectories())
        {
            var childNode = traverseDirectory(subDir, newLevel);
            if (childNode is not null)
            {
                folderNode.Children.Add(childNode);
            }
        }

        // Process .md files (using file name without extension as the note name)
        foreach (var file in dirInfo.GetFiles("*.md"))
        {
            var noteName = Path.GetFileNameWithoutExtension(file.Name);
            var fileNode = new StorageNode
            {
                Name = noteName,
                FullPath = file.FullName,
                IsFile = true,
                Level = newLevel + 1
            };

            folderNode.Children.Add(fileNode);

            // Add note to the flattened search index
            addToIndex(fileNode.Name, fileNode.FullPath, isFile: true);
        }

        return folderNode;
    }

    private void addToIndex(string name, string fullPath, bool isFile)
    {
        if (!_flatIndex.TryGetValue(name, out List<StorageSearchResult>? value1))
        {
            value1 = [];
            _flatIndex[name] = value1;
        }

        value1.Add(new StorageSearchResult(name, fullPath, isFile));
    }
}
