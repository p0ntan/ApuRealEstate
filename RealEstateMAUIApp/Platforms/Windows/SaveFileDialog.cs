// Created by Pontus Åkerberg 2024-10-05
#if WINDOWS
using RealEstateMAUIApp.Services;
using Windows.Storage.Pickers;

namespace RealEstateMAUIApp.Platforms.Windows;

/// <summary>
/// SaveFileDialog for windows, implements ISaveFileDialog.
/// </summary>
public class SaveFileDialog : ISaveFileDialog
{
    public SaveFileDialog() { }

    /// <summary>
    /// Shows the save file dialog for a user to choose a filepath for saving file.
    /// </summary>
    /// <param name="fileTypes"></param>
    /// <returns>Filepath if chosen, null if not or any errors.</returns>
    public async Task<string?> ShowSaveFileDialogAsync(Dictionary<string, List<string>> fileTypes)
    {
        try
        {
            var savePicker = new FileSavePicker();

            foreach (var fileType in fileTypes)
            {
                savePicker.FileTypeChoices.Add(fileType.Key, fileType.Value);
            }

            var hwnd = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            var file = await savePicker.PickSaveFileAsync();

            return file?.Path;
        } catch
        {
            return null;
        } 
    }
}
#endif
