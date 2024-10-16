﻿// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;
using RealEstateMAUIApp.Services;
using RealEstateMAUIApp.Enums;
using RealEstateService;
using UtilitiesLib;

namespace RealEstateMAUIApp;

public partial class MainPage : ContentPage
{
    /// <summary>
    /// The current estate that is created or chosen in the form.
    /// </summary>
    private string _currentFilePath;

    /// <summary>
    /// Flag to control if there are any changes in the form (entry- or pickerfields)
    /// </summary>
    private bool _formHasChanges;

    /// <summary>
    /// Flag to control if there are any changes to the EstateManager (add, update, delete)
    /// </summary>
    private bool _estateManagerHasChanges;

    public MainPage()
    {
        InitializeComponent();
        InitializeGUI();

        _formHasChanges = false;
        _estateManagerHasChanges = false;
        _currentFilePath = string.Empty;

        ExistingEstates.SelectedEstateChanged += SelectedEstateChanges;
    }

    #region Initializeing GUI
    private void InitializeGUI()
    {
        AddEstateTypes();
        AddLegalForms();

        Payment.IsEnabled = false;

        AddChangeControlToControls();
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
    /// Method to add an eventmethod inputChanged to all text-, combo- and checkboxes.
    /// </summary>
    private void AddChangeControlToControls()
    {
        IEnumerable<Entry> allEntries = this.GetVisualTreeDescendants().OfType<Entry>();
        IEnumerable<Picker> allPickers = this.GetVisualTreeDescendants().OfType<Picker>();

        foreach (Entry field in allEntries)
        {
            field.TextChanged += (sender, e) => inputChanged(sender!, e);
        }

        foreach (Picker picker in allPickers)
        {
            if (picker.StyleId == "CountryFilter")
                continue;

            picker.SelectedIndexChanged += (sender, e) => inputChanged(sender!, e);
        }

        IncludePayment.CheckedChanged += (sender, e) => inputChanged(sender!, e);
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
            bool estateSaved = await SaveCurrentEstateToManager();

            if (estateSaved)
            {
                BtnUpdate.Focus();
                ExistingEstates.UpdateList();
                await DisplayAlert("Estate Added", $"Estate added with id {EstateId.Text}", "OK");
            }
                
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK")!;
        }
    }

    /// <summary>
    /// Updates the estate that is in the form and already added to the manager in BLL.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnUpdateEstate(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(EstateId.Text))
            {
                await DisplayAlert("Add estate", "Add estate before updating.", "OK");
                return;
            }

            bool estateSaved = await SaveCurrentEstateToManager();

            if (estateSaved)
            {
                BtnUpdate.Focus();
                ExistingEstates.UpdateList(EstateId.Text);
                await DisplayAlert("Estate Updated", $"Estate updated with id {EstateId.Text}", "OK");
            }   
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK")!;
        }
    }

    /// <summary>
    /// Deletes the current estate in the form if the estate is already in the system/manager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnDeleteEstate(object sender, EventArgs e)
    {
        try
        {
            string estateId = EstateId.Text;

            if (string.IsNullOrEmpty(estateId))
            {
                await DisplayAlert("No chosen estate", "No current estate to delete.", "OK")!;
                return;
            }

            bool wantsToDelete = await DisplayAlert("", $"Are you sure you want to delete estate with id {estateId}?", "Yes", "No");

            if (!wantsToDelete)
                return;

            int idAsInteger = StringConverter.ConvertToInteger(estateId);
            bool success = EstateService.GetInstance().DeleteEstate(idAsInteger);

            if (success)
            {
                ResetForm();
                _estateManagerHasChanges = true;
                ExistingEstates.UpdateList();
                ExistingEstates.SelectNone();
            }
        }
        catch (FormatException)
        {
            await DisplayAlert("", "ID is not a valid integer. Try updating or adding the estate first.", "OK")!;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK")!;
        }
    }

    /// <summary>
    /// When user chooses an estate in the list, this methods gets the id of the chosen estate and updates the form. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void SelectedEstateChanges(object sender, EstateChangedEventArgs e)
    {
        int estateId = e.Value;

        if (_formHasChanges)
        {
            bool wantsToSave = await DisplayAlert("Estate has changes", "Keep changes for the estate in form before changing estate?", "Yes", "No");

            if (wantsToSave)
            {
                bool estateSaved = await SaveCurrentEstateToManager();

                if (!estateSaved)
                    return;

                ExistingEstates.UpdateList();
            }
        }

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
                UpdateGUIForEstateType<ResidentialType>("Area", "Bedrooms");
                break;
            case EstateType.Commercial:
                UpdateGUIForEstateType<CommercialType>("Year Built", "Yearly Revenue");
                break;
            case EstateType.Institutional:
                UpdateGUIForEstateType<InstitutionalType>("Established Year", "No. of Buildings");
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

    /// <summary>
    /// Method changes private field _formHasChanges to true when added to a control.
    /// Used for text-, combo- and checkboxes in form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void inputChanged(object sender, EventArgs e)
    {
        _formHasChanges = true;
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

        if (EstateService.GetInstance().LoadFromFile(filePath.FullPath))
        {
            ResetForm();
            ExistingEstates.Reset();
            ExistingEstates.UpdateList();
            _currentFilePath = filePath.FullPath;
        }
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

        if (EstateService.GetInstance().LoadFromFile(filePath.FullPath))
        {
            ResetForm();
            ExistingEstates.Reset();
            ExistingEstates.UpdateList();
            _currentFilePath = filePath.FullPath;
        }
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
            bool estateSaved = await SaveCurrentEstateToManager();

            if (!estateSaved)
                return;
        }

        EstateService eService = EstateService.GetInstance();

        if (eService.SaveToFile(filePath))
        {
            await DisplayAlert("", "File saved.", "OK");
            _estateManagerHasChanges = false;
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
            bool estateSaved = await SaveCurrentEstateToManager();

            if (!estateSaved)
                return;
        }

        EstateService eService = EstateService.GetInstance();

        if (eService.SaveToFile(filePath))
        {
            await DisplayAlert("", "File saved.", "OK");
            _estateManagerHasChanges = false;
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

    #region Handling methods

    /// <summary>
    /// Returns true if there are any changes in manager or in form.
    /// </summary>
    /// <returns>True if there are changes, false if not.</returns>
    public bool HasUnsavedChanges()
    {
        return _formHasChanges || _estateManagerHasChanges;
    }

    /// <summary>
    /// Handles the unsaved changes, by first asking user what they want to do and then 
    /// </summary>
    /// <returns>True if changes are handled, false if not.</returns>
    private async Task<bool> HandleUnsavedChanges()
    {
        string answer = await DisplayActionSheet("Save changes?", "Cancel", null, "Yes", "No");

        if (answer == null || answer == "Cancel")
            return false;
        else if (answer == "Yes")
            return await SaveChanges();  // Returns false if not saved

        return true;  // Same as pressing no, then changes are handles since they shouln't be saved.
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
            bool estateSaved = await SaveCurrentEstateToManager();

            if (!estateSaved)
                return false;
        }

        if (!EstateService.GetInstance().SaveToFile(filePath))
        {
            await DisplayAlert("", "Data not saved.", "Ok");
            return false;
        }

        _estateManagerHasChanges = false;
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
        try
        {
            string filePath = string.Empty;
            var dialogService = new SaveFileDialogService();
            string? selectedFilePath = await dialogService.ShowSaveFileDialogAsync(fileTypes);

            return selectedFilePath;
        }
        catch
        {
            return string.Empty;
        }
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

    /// <summary>
    /// Update the complete form with data from an estate with given id.
    /// </summary>
    /// <param name="estateId">The id of the estate.</param>
    private void UpdateFormWithEstate(int estateId)
    {
        ResetForm();

        EstateDTO? estate = EstateService.GetInstance().GetEstate(estateId);

        if (estate == null)
            return;

        AddIdAndDisableButtons(estate.ID);
        UpdateEstateTypeInfo(estate);
        UpdateSpecificTypeInfo(estate);
        EstateAddress.SetAddress(estate.Address);
        Seller.SetPerson(estate.Seller);
        Buyer.SetPerson(estate.Buyer);

        if (estate.Buyer.Payment != null)
        {
            IncludePayment.IsChecked = true;
            Payment.SetPayment(estate.Buyer.Payment);
        }

        _formHasChanges = false;
    }

    /// <summary>
    /// Updates form with estate type info.
    /// </summary>
    /// <param name="estate">DTO to update form with.</param>
    private void UpdateEstateTypeInfo(EstateDTO estate)
    {
        (int index, string dataOne, string dataTwo) estateTypeInfo = estate switch
        {
            ResidentialDTO res => ((int)EstateType.Residential, res.Area.ToString(), res.Bedrooms.ToString()),
            CommercialDTO comm => ((int)EstateType.Commercial, comm.YearBuilt.ToString(), comm.YearlyRevenue.ToString()),
            InstitutionalDTO ins => ((int)EstateType.Institutional, ins.EstablishedYear.ToString(), ins.NumberOfBuildings.ToString()),
            _ => (EstateTypePicker.SelectedIndex, "", "")
        };

        EstateTypePicker.SelectedIndex = estateTypeInfo.index;
        txtType1.Text = estateTypeInfo.dataOne;
        txtType2.Text = estateTypeInfo.dataTwo;
    }

    /// <summary>
    /// Updates form with estate specific info.
    /// </summary>
    /// <param name="estate">DTO to update form with.</param>
    private void UpdateSpecificTypeInfo(EstateDTO estate)
    {
        (int index, string dataOne, string dataTwo) specificData = estate switch
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

        SpecificTypePicker.SelectedIndex = specificData.index;
        txtSpecific1.Text = specificData.dataOne;
        txtSpecific2.Text = specificData.dataTwo;
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
    /// <exception cref="FormatException"></exception>
    /// <exception cref="Exception"></exception>
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion

    #region Update GUI methods when changing estate type

    /// <summary>
    /// Updates the GUI with labels and specific for a the given type of estate (ex ResidentialType).
    /// </summary>
    /// <typeparam name="TEstateType">The type of estate, ex. RestidentialType, CommercialType.</typeparam>
    /// <param name="labelOne">String for label one</param>
    /// <param name="labelTwo">String for label two</param>
    private void UpdateGUIForEstateType<TEstateType>(string labelOne, string labelTwo) where TEstateType : Enum
    {
        lblType1.Text = labelOne;
        lblType2.Text = labelTwo;

        // Setting the ItemsSource to the names of the given EstateType (ResidentialType etc.)
        SpecificTypePicker.ItemsSource = Enum.GetNames(typeof(TEstateType));
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
    /// Save current estate as in form to the estatemanager, creates a new one if no estate in _currentEstate.
    /// </summary>
    /// <returns>True if saved, false if not</returns>
    private async Task<bool> SaveCurrentEstateToManager()
    {
        try
        {
            ValidateForm();

            EstateDTO estateDTO = CreateEstateDTO();

            bool success = false;
            int estateID = estateDTO.ID;

            if (estateID == -1)  // Estate is new if id is -1
                (success, estateID) = EstateService.GetInstance().CreateEstate(estateDTO);
            else
                success = EstateService.GetInstance().UpdateEstate(estateDTO);

            if (!success)
                return false;

            AddIdAndDisableButtons(estateID);

            _estateManagerHasChanges = true;
            _formHasChanges = false;

            return true;
        }
        catch (FormatException ex)
        {
            await DisplayAlert("Wrong input", ex.Message, "OK")!;
            return false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK")!;
            return false;
        }
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
        EstateTypePicker.SelectedIndex = 0;

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
        ExistingEstates.Reset();
        _currentFilePath = string.Empty;
        _formHasChanges = false;
        _estateManagerHasChanges = false;
    }
}
