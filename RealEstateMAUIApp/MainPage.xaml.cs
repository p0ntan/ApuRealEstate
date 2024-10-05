// Created by Pontus Åkerberg 2024-10-05
using RealEstateMAUIApp.Enums;
using RealEstateService;

namespace RealEstateMAUIApp;

public partial class MainPage : ContentPage
{
    private readonly EstateService _estateService;

    public MainPage(EstateService estateService)
    {
        InitializeComponent();

        _estateService = estateService;

        InitializeGUI();
    }

    private void InitializeGUI()
    {
        AddEstateTypes();
        AddLegalForms();
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


    private void OnAddEstate(object sender, EventArgs e)
    {
        EstateAddress.ValidateAddress();
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
}
