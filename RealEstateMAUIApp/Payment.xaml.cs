using RealEstateMAUIApp.Enums;

namespace RealEstateMAUIApp;

public partial class Payment : ContentView
{
	public Payment()
	{
		InitializeComponent();

        ResetControl();
    }

    /// <summary>
    /// Resets all textfields and and picker.
    /// </summary>
    public void ResetControl()
    {
        // Get all entry fields
        IEnumerable<Entry> allEntries = this.GetVisualTreeDescendants().OfType<Entry>();

        // Empty all textfields.
        foreach (Entry entry in allEntries)
        {
            entry.Text = string.Empty;
        }

        // Doing a "hard reset"
        PaymentPicker.ItemsSource = Enum.GetNames(typeof(PaymentType));
        PaymentPicker.SelectedIndex = 0;
    }

    /// <summary>
    /// Change the GUI dpending on what paytment type is chosen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PaymentTypeChanged(object sender, EventArgs e)
    {
        PaymentType paymentType = (PaymentType)PaymentPicker.SelectedIndex;

        // Reset visability
        txtPayment2.IsVisible= true;

        txtPayment1.Text = string.Empty;
        txtPayment2.Text = string.Empty;

        switch (paymentType)
        {
            case PaymentType.Bank:
                txtPayment1.Placeholder = "Name";
                txtPayment2.Placeholder = "Account number";
                break;
            case PaymentType.Paypal:
                txtPayment1.Placeholder = "Email";
                txtPayment2.IsVisible = false;
                break;
            case PaymentType.Western_Union:
                txtPayment1.Placeholder = "Name";
                txtPayment2.Placeholder = "Email";
                break;
        }
    }
}