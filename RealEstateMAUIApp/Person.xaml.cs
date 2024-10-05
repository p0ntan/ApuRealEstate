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
}