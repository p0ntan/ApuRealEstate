// Created by Pontus Åkerberg 2024-10-05
using RealEstateMAUIApp.Services;

namespace RealEstateMAUIApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    private void OnNewClicked(object sender, EventArgs e)
    {
        // Reset app
        DisplayAlert("New", "New button clicked!", "OK");
    }

    private async void OnOpenJSONClicked(object sender, EventArgs e)
    {
        // Open chosen JSON file
        try
        {
            PickOptions options = new PickOptions
            {
                PickerTitle = "Please select a JSON file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".json" } },
                { DevicePlatform.iOS, new[] { "public.json" } }
            })
            };

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
                await DisplayAlert("Open JSON", result.FullPath, "OK");
        }
        catch
        {
            await DisplayAlert("Open JSON", "User pushed cancel.", "OK");
        }
    }

    private async void OnOpenXMLClicked(object sender, EventArgs e)
    {
        // Open chosen XML file
        try
        {
            PickOptions options = new PickOptions
            {
                PickerTitle = "Please select a JSON file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".xml" } },
                { DevicePlatform.iOS, new[] { "public.xml" } }
            })};

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
                await DisplayAlert("Open JSON", result.FullPath, "OK");
        }
        catch
        {
            await DisplayAlert("Open JSON", "User pushed cancel.", "OK");
        }
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        // Save current state
    }

    private async void OnSaveAsJSONClicked(object sender, EventArgs e)
    {
        try
        {
            var dialogService = new SaveFileDialogService();

            var fileTypes = new Dictionary<string, List<string>>()
            {
                { "JSON Files", new List<string> { ".json" } },
                { "XML Files", new List<string> { ".xml" } },
                { "Text Files", new List<string> { ".txt" } }
            };

            string? selectedFilePath = await dialogService.ShowSaveFileDialogAsync(fileTypes);

            if (selectedFilePath != null)
                await DisplayAlert("Open JSON", selectedFilePath, "OK");
            else
                await DisplayAlert("Open JSON", "File save was cancelled.", "OK");

        } catch (Exception ex)
        {
            await DisplayAlert("Open JSON", ex.Message, "OK");
        }
    }

    private void OnSaveAsXMLClicked(object sender, EventArgs e)
    {
        // Save current state to chosen XML file

    }

    private void OnExitClicked(object sender, EventArgs e)
    {
        Application.Current?.Quit();
    }
}
