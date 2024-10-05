// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;
using RealEstateService;

namespace RealEstateMAUIApp;

public partial class Address : ContentView
{
    public Address()
    {
        InitializeComponent();
        InitializeAddressGUI();
    }

    private void InitializeAddressGUI()
    {
        CountryPicker.Items.Clear();

        string[] countries = CountryService.GetCountries();

        foreach (string country in countries)
            CountryPicker.Items.Add(country);

        CountryPicker.SelectedItem = "Sweden";  // Default to Sweden
    }

    /// <summary>
    /// Sets the fields in the contorl with a supplied Address class.
    /// </summary>
    /// <param name="addressDTO">AdressDTO to set the adress with.</param>
    public void SetAddress(AddressDTO addressDTO)
    {
        Street.Text = addressDTO.Street;
        City.Text = addressDTO.City;
        ZipCode.Text = addressDTO.ZipCode;
        CountryPicker.SelectedItem = addressDTO.Country;
    }

    /// <summary>
    /// Takes the input fields in the control and creates an Address class.
    /// </summary>
    /// <returns>An AddressDTO</returns>
    public AddressDTO GetAddress()
    {
        string? country = CountryPicker.SelectedItem.ToString();

        if (country == null)
            country = "Sweden";

        AddressDTO addressDTO = new(Street.Text, City.Text, ZipCode.Text, country);

        return addressDTO;
    }

    /// <summary>
    /// Validates the address by controlling that all fields have some text.
    /// </summary>
    public async void ValidateAddress()
    {
        // Fields to control
        Entry[] entryFields =
        [
            Street,
            ZipCode,
            City
        ];

        foreach (Entry field in entryFields)
        {
            if (string.IsNullOrEmpty(field.Text))
            {
                field.Focus();
                await App.Current.MainPage.DisplayAlert("Missing input", $"{field.Placeholder} cannot be empty.", "OK");
                return;
            }
        }
    }
}