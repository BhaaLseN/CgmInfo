using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CgmInfoGui.Views;

namespace CgmInfoGui.Services;

public interface IFileService
{
    Task<string?> OpenFileAsync(string title, params (string Name, string Filter)[] filters);
    Task<string?> SaveFileAsync(string title, params (string Name, string Filter)[] filters);
}
internal sealed class FileService : IFileService
{
    private readonly MainWindow _target;

    public FileService(MainWindow target)
    {
        _target = target;
    }

    public async Task<string?> OpenFileAsync(string title, params (string Name, string Filter)[] filters)
    {
        var files = await _target.StorageProvider.OpenFilePickerAsync(new()
        {
            AllowMultiple = false,
            FileTypeFilter = [.. filters.Select(f => new FilePickerFileType(f.Name) { Patterns = f.Filter.Split('|') })],
            Title = title,
        });
        var pick = files?.FirstOrDefault();
        if (pick is null)
            return null;

        return pick.TryGetLocalPath();
    }
    public async Task<string?> SaveFileAsync(string title, params (string Name, string Filter)[] filters)
    {
        var pick = await _target.StorageProvider.SaveFilePickerAsync(new()
        {
            DefaultExtension = filters.FirstOrDefault().Filter,
            FileTypeChoices = [.. filters.Select(f => new FilePickerFileType(f.Name) { Patterns = f.Filter.Split('|') })],
            ShowOverwritePrompt = true,
            Title = title,
        });
        if (pick is null)
            return null;

        return pick.TryGetLocalPath();
    }
}
