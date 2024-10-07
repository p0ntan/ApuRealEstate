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

	public void UpdateList()
	{
        EstateService estateService = EstateService.GetInstance();

        string[] estates = estateService.GetEstatesAsArrayOfStrings();

        EstateCollection.ItemsSource = estates;
	}

    private void OnEstateSelectionChanged(object sender, SelectedItemChangedEventArgs e)
    {
        EstateDetails.Children.Clear();

        if (EstateCollection.SelectedItem == null)
            return;

        string? estateAsString = EstateCollection.SelectedItem.ToString();

        if (string.IsNullOrEmpty(estateAsString))
            return;

        string idAsString = estateAsString.Split(';')[0]; // Split string and take id from string

        if (!int.TryParse(idAsString, out int idAsInt))
            return;

        EstateService estateService = EstateService.GetInstance();
        List<string>? estate = estateService.GetEstateAsListOfStrings(idAsInt);

        if (estate == null)
            return;

        foreach (var item in estate)
        {
            Label label = new Label()
            {
                Text = item.ToString(),
            };

            EstateDetails.Children.Add(label);
        }

        SelectedEstateChanged?.Invoke(this, new EstateChangedEventArgs(idAsInt));
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
