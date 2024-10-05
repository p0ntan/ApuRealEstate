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

    private void CountryChange(object sender, EventArgs e)
    {
        // Do things when changed
    }

    public void SetAddress(AddressDTO addressDTO)
    {
        Street.Text = addressDTO.Street;
        City.Text = addressDTO.City;
        ZipCode.Text = addressDTO.ZipCode;
        CountryPicker.SelectedItem = addressDTO.Country;
    }

    public AddressDTO GetAddress()
    {
        string? country = CountryPicker.SelectedItem.ToString();

        if (country == null)
            country = "Sweden";

        AddressDTO addressDTO = new(Street.Text, City.Text, ZipCode.Text, country);

        return addressDTO;
    }
}