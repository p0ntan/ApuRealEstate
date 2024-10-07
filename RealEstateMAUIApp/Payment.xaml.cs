// Created by Pontus Åkerberg 2024-10-05
using RealEstateMAUIApp.Enums;
using RealEstateDTO;
using UtilitiesLib;

namespace RealEstateMAUIApp;

/// <summary>
/// Payment is a component for all payment related actions.
/// </summary>
public partial class Payment : ContentView
{
	public Payment()
	{
		InitializeComponent();

        Reset();
    }

    /// <summary>
    /// Resets all textfields and and picker.
    /// </summary>
    public void Reset()
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
    /// Validates the payment by controlling that all entry fields have some text, and amount is a double.
    /// </summary>
    /// <exception cref="FormatException"></exception>
    public void ValidatePayment()
    {
        Amount.Focus();
        StringConverter.ConvertToDouble(Amount.Text, 0, Double.PositiveInfinity);

        // Get all entry fields
        IEnumerable<Entry> allEntries = this.GetVisualTreeDescendants().OfType<Entry>();

        foreach (Entry field in allEntries)
        {
            if (!field.IsVisible)
                continue;

            if (string.IsNullOrEmpty(field.Text))
            {
                field.Focus();
                throw new FormatException($"{field.Placeholder} cannot be empty.");
            }
        }
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
        txtPayment2.IsVisible = true;

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

    /// <summary>
    /// Returns the paymentDTO as in form. Before getting the payment the ValidatePayment should be used to make sure all fields are validated.
    /// </summary>
    /// <returns>Chosen payment with data.</returns>
    /// <exception cref="Exception"></exception>
    public PaymentDTO? GetPayment()
    {
        PaymentType paymentType = (PaymentType)PaymentPicker.SelectedIndex;

        double amount;

        try
        {
            amount = StringConverter.ConvertToDouble(Amount.Text);
        }
        catch
        {
            amount = 0; // Default it to zero
        }

        PaymentDTO? payment = paymentType switch
        {
            PaymentType.Bank => new BankDTO { Amount = amount, Name = txtPayment1.Text, AccountNumber = txtPayment2.Text },
            PaymentType.Paypal => new PaypalDTO {Amount = amount, Email = txtPayment1.Text },
            PaymentType.Western_Union => new WesternUnionDTO { Amount = amount, Name = txtPayment1.Text, Email = txtPayment2.Text },
            _ => null,
        };

        return payment;
    }
}
