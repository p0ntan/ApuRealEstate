// Created by Pontus Åkerberg 2024-10-05
#if WINDOWS
using RealEstateMAUIApp.Services;
using Windows.Storage.Pickers;

namespace RealEstateMAUIApp.Platforms.Windows;


public class SaveFileDialog : ISaveFileDialog
{
    public SaveFileDialog() { }

    public async Task<string?> ShowSaveFileDialogAsync(Dictionary<string, List<string>> fileTypes)
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
    }
}
#endif
