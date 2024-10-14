// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;
using RealEstateMAUIApp.Enums;

namespace RealEstateMAUIApp;

/// <summary>
/// Addressview to be used for places that needs an adress.
/// </summary>
public partial class Address : ContentView
{
    /// <summary>
    /// Default constructor initialized component and GUI.
    /// </summary>
    public Address()
    {
        InitializeComponent();
        InitializeAddressGUI();
    }

    /// <summary>
    /// Initializes GUI by adding countries to picker.
    /// </summary>
    private void InitializeAddressGUI()
    {
        CountryPicker.Items.Clear();
        CountryPicker.ItemsSource = Enum.GetNames(typeof(Countries));
        CountryPicker.SelectedItem = Countries.Sweden;  // Default to Sweden
    }

    /// <summary>
    /// Sets the fields in the Component with a supplied AddressDTO.
    /// </summary>
    /// <param name="addressDTO">AdressDTO to set the adress with.</param>
    public void SetAddress(AddressDTO addressDTO)
    {
        Street.Text = addressDTO.Street;
        City.Text = addressDTO.City;
        ZipCode.Text = addressDTO.ZipCode;
        CountryPicker.SelectedIndex = addressDTO.Country;
    }

    /// <summary>
    /// Takes the input fields in the control and creates an Address class.
    /// </summary>
    /// <returns>An AddressDTO</returns>
    public AddressDTO GetAddress()
    {
        int country = CountryPicker.SelectedIndex;

        AddressDTO addressDTO = new AddressDTO { Street = Street.Text, City = City.Text, ZipCode = ZipCode.Text, Country = country };

        return addressDTO;
    }

    /// <summary>
    /// Validates the address by controlling that all entry fields have some text.
    /// </summary>
    /// <exception cref="FormatException">If string is empty</exception>
    public void ValidateAddress()
    {
        // Get all entry fields
        IEnumerable<Entry> allEntries = this.GetVisualTreeDescendants().OfType<Entry>();

        foreach (Entry field in allEntries)
        {
            if (string.IsNullOrEmpty(field.Text))
            {
                field.Focus();
                throw new FormatException($"{field.Placeholder} cannot be empty.");
            }
        }
    }

    /// <summary>
    /// Resets all textfields and sets default country to Sweden.
    /// </summary>
    public void Reset()
    {
        IEnumerable<Entry> allEntries = this.GetVisualTreeDescendants().OfType<Entry>();

        foreach (Entry field in allEntries)
        {
            field.Text = string.Empty;
        }

        // Set Sweden as default.
        CountryPicker.SelectedItem = Countries.Sweden;
    }
}