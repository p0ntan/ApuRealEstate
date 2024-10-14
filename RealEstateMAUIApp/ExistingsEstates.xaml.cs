// Created by Pontus Åkerberg 2024-10-06
using RealEstateMAUIApp.Enums;
using RealEstateService;

namespace RealEstateMAUIApp;

/// <summary>
/// Shows the existing esates in a list and makes it possible to filter them by countries.
/// </summary>
public partial class ExistingsEstates : ContentView
{
    /// <summary>
    /// Event when the a new estated is selected by the user.
    /// </summary>
    public event EventHandler<EstateChangedEventArgs>? SelectedEstateChanged = null;

    /// <summary>
    /// Currently selected estate.
    /// </summary>
    private int? _selectedID = null;

    public ExistingsEstates()
    {
        InitializeComponent();

        CountryFilter.Items.Clear();
        CountryFilter.ItemsSource = Enum.GetNames(typeof(Countries));
        CountryFilter.SelectedIndex = 0;
    }

    /// <summary>
    /// Deselects the estate in list (if any).
    /// </summary>
    public void SelectNone()
    {
        _selectedID = null;
        EstateCollection.SelectedItem = null;
        EstateDetails.Children.Clear();
    }

    /// <summary>
    /// Updates the list by with the given estateID as a string.
    /// </summary>
    /// <param name="estateID">Id of estate</param>
    public void UpdateList(string estateID)
    {
        if (int.TryParse(estateID, out int id))
            UpdateList(id);
        else
            UpdateList();
    }

    /// <summary>
    /// Updates the list with all estates in system. 
    /// If an ID is set as a parameter that estate details is shown.
    /// </summary>
    /// <param name="estateID"></param>
    public void UpdateList(int? estateID = null)
    {
        EstateService estateService = EstateService.GetInstance();

        string[] estates = estateService.GetEstatesAsArrayOfStrings();

        EstateCollection.ItemsSource = estates;

        if (estateID != null)
            DisplayEstateDetails((int)estateID);
    }

    /// <summary>
    /// Update details when the an estate is chosen in the list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnEstateSelectionChanged(object sender, SelectedItemChangedEventArgs e)
    {
        EstateDetails.Children.Clear();

        if (EstateCollection.SelectedItem == null)
            return;

        string? estateAsString = EstateCollection.SelectedItem.ToString();

        if (string.IsNullOrEmpty(estateAsString))
            return;

        // Try parse id from estate string
        if (!int.TryParse(estateAsString.Split(';')[0], out int estateID))
            return;

        _selectedID = estateID;

        DisplayEstateDetails(estateID);
    }

    /// <summary>
    /// When an estated is chosen in list and then selected by clicking a button, an event is triggered with chosen id.
    /// This can then be used in other places in the GUI, like adding the estate to the form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSelectEstateClicked(object sender, EventArgs e)
    {
        if (_selectedID == null)
            return;

        SelectedEstateChanged?.Invoke(this, new EstateChangedEventArgs((int)_selectedID));
    }

    /// <summary>
    /// Updates list when a country is chosen and then filter button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnFilterByCountryClicked(object sender, EventArgs e)
    {
        if (CountryFilter.SelectedItem == null)
            return;

        string? country = CountryFilter.SelectedItem.ToString();

        EstateService estateService = EstateService.GetInstance();
        string[] foundCountries = estateService.GetEstateByCountry(country);

        EstateCollection.ItemsSource = foundCountries;

        SelectNone();
    }

    /// <summary>
    /// Removes filter and shows all estates in system.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnShowAllClicked(object sender, EventArgs e)
    {
        UpdateList();
    }

    /// <summary>
    /// Updates the details for a given estate by id.
    /// </summary>
    /// <param name="estateID"></param>
    private void DisplayEstateDetails(int estateID)
    {
        EstateDetails.Children.Clear();

        EstateService estateService = EstateService.GetInstance();
        List<string> estateDetails = estateService.GetEstateAsListOfStrings(estateID);

        foreach (var item in estateDetails)
        {
            Label label = new Label()
            {
                Text = item.ToString(),
            };
            EstateDetails.Children.Add(label);
        }
    }

    public void Reset()
    {
        UpdateList();
        SelectNone();
        CountryFilter.SelectedIndex = 0;
    }
}

/// <summary>
/// Args that is used for events when the estate id is needed.
/// </summary>
public class EstateChangedEventArgs : EventArgs
{
    public int Value { get; set; }

    public EstateChangedEventArgs(int value)
    {
        Value = value;
    }
}
