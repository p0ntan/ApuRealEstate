// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;
using System.IO;
using System.Reflection.Emit;

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
    /// Sets the fields in the component with a supplied PersonDTO.
    /// </summary>
    /// <param name="personDTO">PersonDTO to set the fields with.</param>
    public void SetPerson(PersonDTO personDTO)
    {
        FirstName.Text = personDTO.FirstName;
        LastName.Text = personDTO.LastName;
        PersonAddress.SetAddress(personDTO.Address);
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
        PersonDTO person = Title switch
        {
            "Seller" => new SellerDTO { FirstName = FirstName.Text, LastName = LastName.Text, Address = address },
            "Buyer" => new BuyerDTO { FirstName = FirstName.Text, LastName = LastName.Text, Address = address, Payment = payment },
            _ => new SellerDTO { FirstName = FirstName.Text, LastName = LastName.Text, Address = address }
        };

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