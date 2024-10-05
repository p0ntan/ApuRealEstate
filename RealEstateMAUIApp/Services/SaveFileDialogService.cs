// Created by Pontus Åkerberg 2024-10-05

namespace RealEstateMAUIApp.Services;

/// <summary>
/// Service to provide a save dialog that is platform specific.
/// </summary>
public class SaveFileDialogService
{
    /// <summary>
    /// The platform specific save dialog.
    /// </summary>
    private ISaveFileDialog? _saveFileDialog;

    public SaveFileDialogService()
    {
        _saveFileDialog = null;

#if WINDOWS
        _saveFileDialog = new Platforms.Windows.SaveFileDialog();
#endif
    }

    /// <summary>
    /// Shows the save dialog stored in _saveFileDialog to get the filepath from user.
    /// Throws PlatformNotSupportedException if no dialog for current platform.
    /// </summary>
    /// <param name="fileTypes">Which possible files to save to, ex. .JSON and .XML</param>
    /// <returns>Filepath if chosen, null if not.</returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public async Task<string?> ShowSaveFileDialogAsync(Dictionary<string, List<string>> fileTypes)
    {
        if (_saveFileDialog != null)
        {
            return await _saveFileDialog.ShowSaveFileDialogAsync(fileTypes);
        }

        throw new PlatformNotSupportedException("SaveFileDialog is not supported on this platform.");
    }
}
