namespace RealEstateMAUIApp.Services;

public class SaveFileDialogService
{
    private ISaveFileDialog? _saveFileDialog;

    public SaveFileDialogService()
    {
        _saveFileDialog = null;

#if WINDOWS
        _saveFileDialog = new Platforms.Windows.SaveFileDialog();
#endif
    }

    public async Task<string?> ShowSaveFileDialogAsync(Dictionary<string, List<string>> fileTypes)
    {
        if (_saveFileDialog != null)
        {
            return await _saveFileDialog.ShowSaveFileDialogAsync(fileTypes);
        }

        throw new PlatformNotSupportedException("SaveFileDialog is not supported on this platform.");
    }
}
