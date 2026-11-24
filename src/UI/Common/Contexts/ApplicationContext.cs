namespace FSNotes.UI.Common.Contexts;

public class ApplicationContext
{
    public event Action? StorageUpdated;

    public void NotifyStorageUpdated()
    {
        StorageUpdated?.Invoke();
    }
}