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
            DisplayAlert("New", "File saved as XML!", "OK");
        }

        private void OnOpenJSONClicked(object sender, EventArgs e)
        {
            // Open chosen JSON file
        }

        private void OnOpenXMLClicked(object sender, EventArgs e)
        {
            // Open chosen XML file
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            // Save current state
        }

        private void OnSaveAsJSONClicked(object sender, EventArgs e)
        {
            // Save current state to chosen JSON file
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
