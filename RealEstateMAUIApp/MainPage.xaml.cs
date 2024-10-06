// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;
using RealEstateMAUIApp.Enums;
using RealEstateService;
using UtilitiesLib;

namespace RealEstateMAUIApp;

public partial class MainPage : ContentPage
{
    private readonly EstateService _estateService;

    private bool _formHasChanges;

    public MainPage(EstateService estateService)
    {
        InitializeComponent();

        _estateService = estateService;

        InitializeGUI();

        _formHasChanges = false;
    }

    private void InitializeGUI()
    {
        AddEstateTypes();
        AddLegalForms();

        Payment.IsEnabled = false;
    }

    /// <summary>
    /// Add estate types to picker.
    /// </summary>
    private void AddEstateTypes()
    {
        EstateTypePicker.Items.Clear();
        EstateTypePicker.ItemsSource = Enum.GetNames(typeof(EstateType));
        EstateTypePicker.SelectedIndex = 0;
    }

    /// <summary>
    /// Add legal form to picker.
    /// </summary>
    private void AddLegalForms()
    {
        LegalFormPicker.Items.Clear();
        LegalFormPicker.ItemsSource = Enum.GetNames(typeof(LegalForm));
        LegalFormPicker.SelectedIndex = 0;
    }

    /// <summary>
    /// Validates the inputs before creating an DTO for new estates.
    /// Sends the created DTO to the servicelayer, recives a boolean if created and the new id for the estate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnAddEstate(object sender, EventArgs e)
    {
        try
        {
            // Validate form
            ValidateForm();

            // Create DTO
            EstateCreateDTO estateDTO = CreateEstateDTO();

            // Send DTO to Servicelayer
            (bool success, int newId) response = _estateService.CreateEstate(estateDTO);

            // Message user, and update form with new ID
            if (response.success)
            {
                UpdateGuiForExistingEstate(response.newId);
                await DisplayAlert("Estate added", $"Estate added with id {response.newId}", "OK");
            }
            else
                await DisplayAlert("Estate not added", "Control inputs, couldn't create estate.", "OK");
        }
        catch (FormatException ex)
        {
            await DisplayAlert("Wrong input", ex.Message, "OK")!;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK")!;
        }
    }

    /// <summary>
    /// Updates the GUI by setting the ID to given id and disables buttons.
    /// </summary>
    /// <param name="estateId">Estate id to show in form.</param>
    private void UpdateGuiForExistingEstate(int estateId)
    {
        EstateId.Text = $"ID: {estateId}";
        EstateTypePicker.IsEnabled = false;
        SpecificTypePicker.IsEnabled = false;
        BtnAdd.IsEnabled = false;
    }

    private void OnUpdateEstate(object sender, EventArgs e)
    {
        // Validate

        // Create DTO

        // Send DTO to Servicelayer
        EstateAddress.ValidateAddress();
    }

    private async void OnDeleteEstate(object sender, EventArgs e)
    {
        string estateId = EstateId.Text;

        if (string.IsNullOrEmpty(estateId))
            await DisplayAlert("No chosen estate", "No current estate to delete.", "OK")!;



        // Send ID to Service for deletion (no need for DTO?)

        // Reset form
    }


    /// <summary>
    /// Resets the form when button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnResetForm(object sender, EventArgs e)
    {
        ResetForm();
    }

    /// <summary>
    /// Validates the form, 
    /// </summary>
    /// <exception cref="FormatException">Rethrows exception if any.</exception>
    private void ValidateForm()
    {
        try
        {
            // Validate Estate Address 
            EstateAddress.ValidateAddress();

            // Validate input fields for estate info, focus on each one first in case they fail.
            txtType1.Focus();
            StringConverter.ConvertToInteger(txtType1.Text, 0, 2500);
            txtType2.Focus();
            StringConverter.ConvertToInteger(txtType2.Text);
            txtSpecific1.Focus();
            StringConverter.ConvertToInteger(txtSpecific1.Text);
            txtSpecific2.Focus();
            StringConverter.ConvertToInteger(txtSpecific2.Text);

            // Validate payment if included
            if (IncludePayment.IsChecked)
                Payment.ValidatePayment();

            EstateTypePicker.Focus();
        }
        catch (FormatException ex)
        {
            throw new FormatException(ex.Message);
        }
    }

    /// <summary>
    /// Creates a DTO for creating new estates. Method should run after the form as been validated.
    /// </summary>
    /// <returns>An comlete estate as DTO</returns>
    private EstateCreateDTO CreateEstateDTO()
    {
        PaymentDTO? paymentDTO = null;

        if (IncludePayment.IsChecked)
            paymentDTO = Payment.GetPayment();

        EstateCreateDTO estateDTO = new(
            null,
            EstateTypePicker.SelectedIndex,
            SpecificTypePicker.SelectedIndex,
            LegalFormPicker.SelectedIndex,
            EstateAddress.GetAddress(),
            Seller.GetPerson(),
            Buyer.GetPerson(paymentDTO),
            StringConverter.ConvertToInteger(txtType1.Text, 0, 2500),
            StringConverter.ConvertToInteger(txtType2.Text),
            StringConverter.ConvertToInteger(txtSpecific1.Text),
            StringConverter.ConvertToInteger(txtSpecific2.Text)
        );

        return estateDTO;
    }

    /// <summary>
    /// Enables or disables payment if cheked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void IncludePaymentCheckbox(object sender, EventArgs e)
    {
        Payment.IsEnabled = IncludePayment.IsChecked;
    }

    /// <summary>
    /// Update form when changeing type of estate (Residential etc.)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EstateTypeChanged(object sender, EventArgs e)
    {
        EstateType estateType = (EstateType)EstateTypePicker.SelectedIndex;

        SpecificTypePicker.SelectedIndex = -1;  // Set to -1 if index is out of range for other estattype

        switch (estateType)
        {
            case EstateType.Residential:
                UpdateGUIForResidentials();
                break;
            case EstateType.Commercial:
                UpdateGUIForCommercials();
                break;
            case EstateType.Institutional:
                UpdateGUIForInstitutionals();
                break;
        }
    }

    /// <summary>
    /// Update form when changeing type of specific estate (Villa, Hotel etc.)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecificTypeChanged(object sender, EventArgs e)
    {
        EstateType estateType = (EstateType)EstateTypePicker.SelectedIndex;

        // Setting the "default" value to Ownership when chaning estate type. 
        // Rental and Tenement changes this by itself but can be overrrun.
        LegalFormPicker.SelectedIndex = (int)LegalForm.Ownership;

        switch (estateType)
        {
            case EstateType.Residential:
                UpdateGUIForSpecificResidential();
                break;
            case EstateType.Commercial:
                UpdateGUIForSpecificCommercial();
                break;
            case EstateType.Institutional:
                UpdateGUIForSpecificInstitutional();
                break;
        }
    }

    /// <summary>
    /// Updates the GUI with details specific for all Residentials.
    /// </summary>
    private void UpdateGUIForResidentials()
    {
        lblType1.Text = "Area";
        lblType2.Text = "Bedrooms";

        // Changing specific type datasource triggers methods for updating the specifics
        // through the event cmbSpecificType_SelectedIndexChanged method.
        SpecificTypePicker.ItemsSource = Enum.GetNames(typeof(ResidentialType));
        SpecificTypePicker.SelectedIndex = 0;
    }

    /// <summary>
    /// Updates the GUI with details specific for all Commercials.
    /// </summary>
    private void UpdateGUIForCommercials()
    {

        lblType1.Text = "Year Built";
        lblType2.Text = "Yearly Revenue";

        // Changing specific type datasource triggers methods for updating the specifics
        // through the event cmbSpecificType_SelectedIndexChanged method.
        SpecificTypePicker.ItemsSource = Enum.GetNames(typeof(CommercialType));
        SpecificTypePicker.SelectedIndex = 0;
    }

    /// <summary>
    /// Updates the GUI with details specific for all Institutionals.
    /// </summary>
    private void UpdateGUIForInstitutionals()
    {
        lblType1.Text = "Established Year";
        lblType2.Text = "No. of Buildings";

        // Changing specific type datasource triggers methods for updating the specifics
        // through the event cmbSpecificType_SelectedIndexChanged method.
        SpecificTypePicker.ItemsSource = Enum.GetNames(typeof(InstitutionalType));
        SpecificTypePicker.SelectedIndex = 0;
    }

    /// <summary>
    /// Updates the GUI for a specific type of Residential.
    /// </summary>
    private void UpdateGUIForSpecificResidential()
    {
        ResidentialType residentialType = (ResidentialType)SpecificTypePicker.SelectedIndex;

        switch (residentialType)
        {
            case ResidentialType.Villa:
                UpdateSpecifics("Floors", "Plot Area");
                ImageEstate.Source = "house.jpg";
                break;
            case ResidentialType.Rowhouse:
                UpdateSpecifics("Floors", "Plot Area");
                ImageEstate.Source = "rowhouse.jpg";
                break;
            case ResidentialType.Rental:
                UpdateSpecifics("On Floor", "Monthly Cost");
                ImageEstate.Source = "apartment.jpg";
                LegalFormPicker.SelectedIndex = (int)LegalForm.Rental;
                break;
            case ResidentialType.Tenement:
                UpdateSpecifics("On Floor", "Monthly Cost");
                ImageEstate.Source = "apartment.jpg";
                LegalFormPicker.SelectedIndex = (int)LegalForm.Tenement;
                break;
        }
    }

    /// <summary>
    /// Updates the GUI for a specific type of Commercial.
    /// </summary>
    private void UpdateGUIForSpecificCommercial()
    {
        CommercialType residentialType = (CommercialType)SpecificTypePicker.SelectedIndex;

        switch (residentialType)
        {
            case CommercialType.Factory:
                UpdateSpecifics("Production Capacity", "No. of Employees");
                ImageEstate.Source = "factory.jpg";
                break;
            case CommercialType.Shop:
                UpdateSpecifics("Customer Capacity", "Storage Area");
                ImageEstate.Source = "shop.jpg";
                break;
            case CommercialType.Hotel:
                UpdateSpecifics("No. of Beds", "No. of Parking Spots");
                ImageEstate.Source = "hotel.jpg";
                break;
            case CommercialType.Warehouse:
                UpdateSpecifics("Storage Area", "No. of Loading Docs");
                ImageEstate.Source = "warehouse.jpg";
                break;
        }
    }

    /// <summary>
    /// Updates the GUI for a specific type of Institutional.
    /// </summary>
    private void UpdateGUIForSpecificInstitutional()
    {
        InstitutionalType residentialType = (InstitutionalType)SpecificTypePicker.SelectedIndex;

        switch (residentialType)
        {
            case InstitutionalType.School:
                UpdateSpecifics("No. of Teachers", "Student Capacity");
                ImageEstate.Source = "school.jpg";
                break;
            case InstitutionalType.University:
                UpdateSpecifics("Campus Area", "Student Capacity");
                ImageEstate.Source = "school.jpg";
                break;
            case InstitutionalType.Hospital:
                UpdateSpecifics("No. of Beds", "No. of Parking Spots");
                ImageEstate.Source = "hospital.jpg";
                break;
        }
    }

    /// <summary>
    /// Updates labels for the specific data.
    /// </summary>
    /// <param name="firstLabel">String for first label.</param>
    /// <param name="secondLabel">String for second label.</param>
    private void UpdateSpecifics(string firstLabel, string secondLabel)
    {
        lblSpecific1.Text = firstLabel;
        lblSpecific2.Text = secondLabel;
    }

    /// <summary>
    /// Resets all fields in the form.
    /// </summary>
    private void ResetForm()
    {
        EstateId.Text = string.Empty;
        EstateAddress.Reset();
        Buyer.Reset();
        Seller.Reset();
        Payment.Reset();

        IEnumerable<Entry> allEntries = this.GetVisualTreeDescendants().OfType<Entry>();

        foreach (Entry field in allEntries)
        {
            field.Text = string.Empty;
        }

        LegalFormPicker.ItemsSource = Enum.GetNames(typeof(LegalForm));
        EstateTypePicker.ItemsSource = Enum.GetNames(typeof(EstateType));

        IncludePayment.IsChecked = false;
        EstateTypePicker.IsEnabled = true;
        SpecificTypePicker.IsEnabled = true;
        BtnAdd.IsEnabled = true;

        _formHasChanges = false;

        EstateTypePicker.Focus();
    }
}
