// Created by Pontus Åkerberg 2024-10-05

namespace RealEstateMAUIApp
{
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                await DisplayAlert("Open JSON", "User pushed cancel.", "OK");
            }
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            // Save current state
        }

        private void OnSaveAsJSONClicked(object sender, EventArgs e)
        {
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
}
