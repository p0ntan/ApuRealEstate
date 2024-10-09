// Created by Pontus Åkerberg 2024-10-06
using RealEstateService;

namespace RealEstateMAUIApp;

public partial class ExistingsEstates : ContentView
{
    public event EventHandler<EstateChangedEventArgs>? SelectedEstateChanged = null;

    public ExistingsEstates()
    {
        InitializeComponent();
    }

    public void SelectNone()
    {
        EstateCollection.SelectedItem = null;
    }

    public void UpdateList(int? estateID = null)
    {
        EstateService estateService = EstateService.GetInstance();

        string[] estates = estateService.GetEstatesAsArrayOfStrings();

        EstateCollection.ItemsSource = estates;

        if (estateID != null)
            DisplayEstateDetails((int)estateID);
    }

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

        DisplayEstateDetails(estateID);

        SelectedEstateChanged?.Invoke(this, new EstateChangedEventArgs(estateID));
    }

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
}

public class EstateChangedEventArgs : EventArgs
{
    public int Value { get; set; }

    public EstateChangedEventArgs(int value)
    {
        Value = value;
    }
}
