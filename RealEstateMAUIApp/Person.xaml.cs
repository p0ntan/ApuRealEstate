using RealEstateBLL.Persons;
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
        string firstName = FirstName.Text;
        string lastName = LastName.Text;
        AddressDTO address = PersonAddress.GetAddress();
        PersonDTO person = new (firstName, lastName, address, payment);

        return person;
    }
}