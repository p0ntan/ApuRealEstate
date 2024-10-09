// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;
using RealEstateMAUIApp.Services;
using RealEstateMAUIApp.Enums;
using RealEstateService;
using UtilitiesLib;

namespace RealEstateMAUIApp;

public partial class MainPage : ContentPage
{
    private string _currentFilePath;
    private bool _formHasChanges;

    public MainPage()
    {
        InitializeComponent();
        InitializeGUI();

        _formHasChanges = false;
        _currentFilePath = string.Empty;

        ExistingEstates.SelectedEstateChanged += SelectedEstateChanges;
    }

    #region Initializeing GUI
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
    #endregion

    #region Form Events
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

            EstateDTO estateDTO = CreateEstateDTO();

            // Use singleton for keeping same instance of EstateService
            EstateService estateService = EstateService.GetInstance();
            (bool success, int newId) response = estateService.CreateEstate(estateDTO);

            // Message user, and update form with new ID
            if (response.success)
            {
                AddIdAndDisableButtons(response.newId);
                ExistingEstates.UpdateList();
                BtnUpdate.Focus();
                await DisplayAlert("Estate Added", $"Estate added with id {response.newId}", "OK");
            }
            else
                await DisplayAlert("Estate Not Added", "Control inputs, couldn't create estate.", "OK");
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

    private async void OnUpdateEstate(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(EstateId.Text))
            {
                await DisplayAlert("Add estate", "Add estate before updating.", "OK");
                return;
            }

            ValidateForm();

            EstateDTO estateDTO = CreateEstateDTO();
            EstateService estateService = EstateService.GetInstance();
            bool success = estateService.UpdateEstate(estateDTO);

            if (success)
            {
                ExistingEstates.UpdateList(estateDTO.ID);
                BtnUpdate.Focus();

                await DisplayAlert("Estate Updated", $"Estate updated with id {EstateId.Text}", "OK");
            }
            else
                await DisplayAlert("Estate Not Updated", "Control inputs, couldn't update estate.", "OK");
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

    private async void OnDeleteEstate(object sender, EventArgs e)
    {
        try
        {
            string estateId = EstateId.Text;

            if (string.IsNullOrEmpty(estateId))
                await DisplayAlert("No chosen estate", "No current estate to delete.", "OK")!;

            int idAsInteger = StringConverter.ConvertToInteger(estateId);

            EstateService estateService = EstateService.GetInstance();
            bool success = estateService.DeleteEstate(idAsInteger);

            if (success)
            {
                ResetForm();
                ExistingEstates.UpdateList();
                ExistingEstates.SelectNone();
                await DisplayAlert("Estate Deleted", $"Estate with id {estateId} deleted", "OK")!;
            }
        }
        catch (FormatException)
        {
            await DisplayAlert("Wrong input", "ID is not a valid integer", "OK")!;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK")!;
        }
    }

    private async void SelectedEstateChanges(object sender, EstateChangedEventArgs e)
    {
        int estateId = e.Value;

        if (_formHasChanges)
            await DisplayAlert("Form has changes.", "Save before changing estate?", "Yes", "No");

        UpdateFormWithEstate(estateId);
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
    private void EstateTypeIndexChanged(object sender, EventArgs e)
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
    private void SpecificTypeIndexChanged(object sender, EventArgs e)
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
    #endregion

    #region Menu Events
    /// <summary>
    /// Controls if there is any unsaved data, asking the user if it should be saved then resets the application to it's original state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnNewClicked(object sender, EventArgs e)
    {
        try
        {
            if (HasUnsavedChanges())
            {
                bool changesHandled = await HandleUnsavedChanges();

                if (!changesHandled)
                    return;
            }

            ResetApplication();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message.ToString(), "OK");
        }
    }

    /// <summary>
    /// Controls if there is any unsaved data, asking the user if it should be saved then opens dialog to open a new file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnOpenJSONClicked(object sender, EventArgs e)
    {
        if (HasUnsavedChanges())
        {
            bool changesHandled = await HandleUnsavedChanges();

            if (!changesHandled)
                return;
        }

        PickOptions options = new PickOptions
        {
            PickerTitle = "Please select a JSON file",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".json" } },
            { DevicePlatform.iOS, new[] { "public.json" } }
        })
        };

        var filePath = await FilePicker.Default.PickAsync(options);

        if (filePath?.FullPath == null)
            return;

        EstateService eService = EstateService.GetInstance();
        eService.LoadFromFile(filePath.FullPath);
    }

    /// <summary>
    /// Opens a file dialog to open a XML file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnOpenXMLClicked(object sender, EventArgs e)
    {
        if (HasUnsavedChanges())
        {
            bool changesHandled = await HandleUnsavedChanges();

            if (!changesHandled)
                return;
        }

        PickOptions options = new PickOptions
        {
            PickerTitle = "Please select a XML file",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".xml" } },
            { DevicePlatform.iOS, new[] { "public.xml" } }
        })
        };
        var filePath = await FilePicker.Default.PickAsync(options);

        if (filePath?.FullPath == null)
            return;

        EstateService.GetInstance().LoadFromFile(filePath.FullPath);
    }

    /// <summary>
    /// Saves the current state of application if the _currentFilePath is set. Else it uses same logic as for Save As.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await SaveChanges();
    }

    /// <summary>
    /// Saves the current state to a JSON-file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnSaveAsJSONClicked(object sender, EventArgs e)
    {
        var fileTypes = new Dictionary<string, List<string>>()
        {
            { "JSON Files (*.json)", new List<string> { ".json" } },
        };
        string? filePath = await GetFilePathUsingDialog(fileTypes);

        if (string.IsNullOrEmpty(filePath))
        {
            await DisplayAlert("", "No file chosen.", "OK");
            return;
        }

        if (_formHasChanges)
        {
            bool estateSaved = true;

            if (!estateSaved)
                return;
        }

        EstateService eService = EstateService.GetInstance();

        if (eService.SaveToFile(filePath))
        {
            await DisplayAlert("", "File saved.", "OK");
            _currentFilePath = filePath;
        }
        else
            await DisplayAlert("", "Problem when saving file, control form input and filename.", "OK");
    }

    /// <summary>
    /// Saves the current state of application as XML.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnSaveAsXMLClicked(object sender, EventArgs e)
    {
        var fileTypes = new Dictionary<string, List<string>>()
        {
            { "XML files (*.xml)", new List<string> { ".XML" } },
        };
        string? filePath = await GetFilePathUsingDialog(fileTypes);

        if (string.IsNullOrEmpty(filePath))
        {
            await DisplayAlert("", "No file chosen.", "OK");
            return;
        }

        if (_formHasChanges)
        {
            bool estateSaved = true;

            if (!estateSaved)
                return;
        }

        EstateService eService = EstateService.GetInstance();

        if (eService.SaveToFile(filePath))
        {
            await DisplayAlert("", "File saved.", "OK");
            _currentFilePath = filePath;
        }
        else
            await DisplayAlert("", "Problem when saving file, control form input and filename.", "OK");
    }

    /// <summary>
    /// Controls if there is any unsaved data, asking the user if it should be saved then exits the application.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnExitClicked(object sender, EventArgs e)
    {
        if (HasUnsavedChanges())
        {
            bool changesHandled = await HandleUnsavedChanges();

            if (!changesHandled)
                return;
        }

        Application.Current?.Quit();
    }
    #endregion

    /// <summary>
    /// Returns true if there are any changes in manager or in form.
    /// </summary>
    /// <returns>True if there are changes, false if not.</returns>
    public bool HasUnsavedChanges()
    {
        return true;
    }

    /// <summary>
    /// Handles the unsaved changes, by first asking user what they want to do and then 
    /// </summary>
    /// <returns>True if changes are handled, false if not.</returns>
    private async Task<bool> HandleUnsavedChanges()
    {
        // String is changed if there are changes in the current form
        string changesInForm = string.Empty;

        if (_formHasChanges)
            changesInForm = "\n\n(Includes the current estate in the form.)";

        bool wantsToSave = await DisplayAlert("Unsaved changes", "Do you want to save your unsaved changes?" + changesInForm, "Yes", "No");

        if (wantsToSave)
            return await SaveChanges();  // Returns false if not saved

        return true;
    }

    /// <summary>
    /// Saves the changes by first getting the filepath, saves the current changes in form (if any) to manager.
    /// Then saves the estatemanager to file.
    /// </summary>
    /// <returns>True if changes are saved, false if not.</returns>
    private async Task<bool> SaveChanges()
    {
        string? filePath = await GetFilePathForSaving();

        if (string.IsNullOrEmpty(filePath))
        {
            await DisplayAlert("", "No file chosen", "Ok");
            return false;
        }

        if (_formHasChanges)
        {
            bool estateSaved = true;

            if (!estateSaved)
                return false;
        }

        EstateService eService = EstateService.GetInstance();

        if (!eService.SaveToFile(filePath))
        {
            await DisplayAlert("", "Data not saved.", "Ok");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the filepath by first checking if there is a current file opened, if not use dialog.
    /// Gives the user option to choose json or xml.
    /// </summary>
    /// <returns>The filepath to file.</returns>
    private async Task<string?> GetFilePathForSaving()
    {
        string? filePath = _currentFilePath;

        if (string.IsNullOrEmpty(filePath))
        {
            var fileTypes = new Dictionary<string, List<string>>()
            {
                { "JSON Files (*.json)", new List<string> { ".json" } },
                { "XML files (*.xml)", new List<string> { ".xml" } },
            };
            filePath = await GetFilePathUsingDialog(fileTypes);
        }

        return filePath;
    }

    /// <summary>
    /// Gets the filepath from a user with a file dialog.
    /// </summary>
    /// <returns>Filepath as string</returns>
    private async Task<string?> GetFilePathUsingDialog(Dictionary<string, List<string>> fileTypes)
    {
        string filePath = string.Empty;
        var dialogService = new SaveFileDialogService();
        string? selectedFilePath = await dialogService.ShowSaveFileDialogAsync(fileTypes);

        return selectedFilePath;
    }

    /// <summary>
    /// Updates the GUI by setting the ID to given id and disables buttons.
    /// </summary>
    /// <param name="estateId">Estate id to show in form.</param>
    private void AddIdAndDisableButtons(int estateId)
    {
        EstateId.Text = estateId.ToString();

        EstateTypePicker.IsEnabled = false;
        SpecificTypePicker.IsEnabled = false;
        BtnAdd.IsEnabled = false;
    }

    private void UpdateFormWithEstate(int estateId)
    {
        ResetForm();

        EstateDTO? estate = EstateService.GetInstance().GetEstate(estateId);

        if (estate == null) return;

        AddIdAndDisableButtons(estate.ID);

        var estateTypeInfo = GetEstateTypeInfo(estate);
        EstateTypePicker.SelectedIndex = estateTypeInfo.estateTypeIndex;
        txtType1.Text = estateTypeInfo.typeOneData;
        txtType2.Text = estateTypeInfo.typeTwoData;

        var specificData = GetSpecificTypeInfo(estate);
        SpecificTypePicker.SelectedIndex = specificData.specificIndex;
        txtSpecific1.Text = specificData.specificOneData;
        txtSpecific2.Text = specificData.specificTwoData;

        EstateAddress.SetAddress(estate.Address);
        Seller.SetPerson(estate.Seller);
        Buyer.SetPerson(estate.Buyer);

        if (estate.Buyer.Payment != null)
        {
            IncludePayment.IsChecked = true;
            Payment.SetPayment(estate.Buyer?.Payment);
        }
    }

    private (int estateTypeIndex, string typeOneData, string typeTwoData) GetEstateTypeInfo(EstateDTO estate)
    {
        return estate switch
        {
            ResidentialDTO res => ((int)EstateType.Residential, res.Area.ToString(), res.Bedrooms.ToString()),
            CommercialDTO comm => ((int)EstateType.Commercial, comm.YearBuilt.ToString(), comm.YearlyRevenue.ToString()),
            InstitutionalDTO ins => ((int)EstateType.Institutional, ins.EstablishedYear.ToString(), ins.NumberOfBuildings.ToString()),
            _ => (EstateTypePicker.SelectedIndex, "", "")
        };
    }

    private (int specificIndex, string specificOneData, string specificTwoData) GetSpecificTypeInfo(EstateDTO estate)
    {
        return estate switch
        {
            RowhouseDTO specs => ((int)ResidentialType.Rowhouse, specs.Floors.ToString(), specs.PlotArea.ToString()),
            VillaDTO specs => ((int)ResidentialType.Villa, specs.Floors.ToString(), specs.PlotArea.ToString()),
            RentalDTO specs => ((int)ResidentialType.Rental, specs.OnFloor.ToString(), specs.MonthlyCost.ToString()),
            TenementDTO specs => ((int)ResidentialType.Tenement, specs.OnFloor.ToString(), specs.MonthlyCost.ToString()),
            FactoryDTO specs => ((int)CommercialType.Factory, specs.ProductionCapacity.ToString(), specs.NumberOfEmployees.ToString()),
            ShopDTO specs => ((int)CommercialType.Shop, specs.CustomerCapacity.ToString(), specs.StorageArea.ToString()),
            HotelDTO specs => ((int)CommercialType.Hotel, specs.NumberOfBeds.ToString(), specs.NumberOfParkingSpots.ToString()),
            WarehouseDTO specs => ((int)CommercialType.Warehouse, specs.StorageArea.ToString(), specs.NumberOfLoadingDocks.ToString()),
            SchoolDTO specs => ((int)InstitutionalType.School, specs.NumberOfTeachers.ToString(), specs.StudentCapacity.ToString()),
            UniversityDTO specs => ((int)InstitutionalType.University, specs.CampusArea.ToString(), specs.StudentCapacity.ToString()),
            HospitalDTO specs => ((int)InstitutionalType.Hospital, specs.NumberOfBeds.ToString(), specs.NumberOfParkingSpots.ToString()),
            _ => (SpecificTypePicker.SelectedIndex, "", "")
        };
    }

    /// <summary>
    /// Resets the form when button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnResetForm(object sender, EventArgs e)
    {
        ExistingEstates.SelectNone();
        ResetForm();
    }

    /// <summary>
    /// Validates the form, 
    /// </summary>
    /// <exception cref="FormatException">Rethrows exception if any.</exception>
    private void ValidateForm()
    {
        // Used for better output for user when an error occurs. Only for estate info/details
        string currentLabel = string.Empty;

        try
        {
            // Validate Estate Address 
            EstateAddress.ValidateAddress();

            (Label lbl, Entry txt)[] integerFields = 
                [(lblType1, txtType1),(lblType2, txtType2), (lblSpecific1, txtSpecific1), (lblSpecific2, txtSpecific2)];

            // Validate input fields for estate info, focus on each one first in case they fail.
            foreach (var item in integerFields)
            {
                item.txt.Focus();
                currentLabel = item.lbl.Text + " ";

                if (item.txt == txtType1)
                    StringConverter.ConvertToInteger(txtType1.Text, 0, 2500);
                else
                    StringConverter.ConvertToInteger(item.txt.Text);
            }

            currentLabel = string.Empty;  // Reset after use

            // Validate payment if included
            if (IncludePayment.IsChecked)
                Payment.ValidatePayment();
        }
        catch (FormatException ex)
        {
            throw new FormatException(currentLabel + ex.Message);
        }
    }

    /// <summary>
    /// Creates a DTO for creating new estates. Method should run after the form as been validated.
    /// </summary>
    /// <returns>An comlete estate as DTO</returns>
    private EstateDTO CreateEstateDTO()
    {
        try
        {
            int estateId = !string.IsNullOrEmpty(EstateId.Text) ? StringConverter.ConvertToInteger(EstateId.Text) : -1;

            EstateType eType = (EstateType)EstateTypePicker.SelectedIndex;
            int specificIndex = SpecificTypePicker.SelectedIndex;
            int typeOne = StringConverter.ConvertToInteger(txtType1.Text, 0, 2500);
            int typeTwo = StringConverter.ConvertToInteger(txtType2.Text);
            int specOne = StringConverter.ConvertToInteger(txtSpecific1.Text);
            int specTwo = StringConverter.ConvertToInteger(txtSpecific2.Text);

            PaymentDTO? paymentDTO = null;

            if (IncludePayment.IsChecked)
                paymentDTO = Payment.GetPayment();

            EstateDTOBuilder dtoBuilder = new EstateDTOBuilder(eType, specificIndex)
                .AddID(estateId)
                .AddLegalForm(LegalFormPicker.SelectedIndex)
                .AddAddress(EstateAddress.GetAddress())
                .AddSeller((SellerDTO)Seller.GetPerson())
                .AddBuyer((BuyerDTO)Buyer.GetPerson(paymentDTO))
                .AddEstateTypeDetails((typeOne, typeTwo))
                .AddEstateSpecificDetails((specOne, specTwo));

            EstateDTO estateDTO = dtoBuilder.Build();

            return estateDTO;
        }
        catch (FormatException ex)
        {
            throw new FormatException(ex.Message);
        }
    }

    #region Update GUI methods when changing estate type

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
    #endregion

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

    /// <summary>
    /// Resets the whole application.
    /// </summary>
    private void ResetApplication()
    {
        ResetForm();

        EstateService.GetInstance().ResetManager();

        // Updating list after reseting manager will clear list.
        ExistingEstates.UpdateList();  
        _currentFilePath = string.Empty;
        _formHasChanges = false;
    }
}
