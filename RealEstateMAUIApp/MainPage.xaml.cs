// Created by Pontus Åkerberg 2024-10-05
using RealEstateService;

namespace RealEstateMAUIApp;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();

        CountryPicker.Items.Clear();

        string[] countries = CountryService.GetCountries();

        foreach (string country in countries)
            CountryPicker.Items.Add(country);
    }

    private void CountryChange(object sender, EventArgs e)
    {
        PickerIndex.Text = CountryPicker.SelectedIndex.ToString();
        PickerCountry.Text = CountryPicker.SelectedItem.ToString();
    }
}
