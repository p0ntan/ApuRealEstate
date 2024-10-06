// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;

namespace RealEstateMAUIApp;

public partial class Person : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(Person),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is Person person)
                {
                    person.TitleLabel.Text = (string)newValue;
                }
            });

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public Person()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Returns a created personDTO with from input in textcontrollers.
    /// </summary>
    /// <returns>The newly created person.</returns>
    /// <param name="payment">Payment to add to a person.</param>
    /// <returns>PersonDTO</returns>
    public PersonDTO GetPerson(PaymentDTO? payment = null)
    {
        AddressDTO address = PersonAddress.GetAddress();
        PersonDTO person = new (FirstName.Text, LastName.Text, address, payment);

        return person;
    }

    /// <summary>
    /// Resets all textboxes and address controller.
    /// </summary>

    public void Reset()
    {
        IEnumerable<Entry> allEntries = this.GetVisualTreeDescendants().OfType<Entry>();

        foreach (Entry field in allEntries)
        {
            field.Text = string.Empty;
        }

        PersonAddress.Reset();
    }
}