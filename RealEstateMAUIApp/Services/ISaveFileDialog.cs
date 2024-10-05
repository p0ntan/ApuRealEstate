// Created by Pontus Åkerberg 2024-10-05

namespace RealEstateMAUIApp.Services;

/// <summary>
/// Interface for to use for all classes that implements a platform specific file dialog.
/// Implementations in Platforms folder, see Platforms/Windows/SaveFileDialog.cs for example.
/// </summary>
public interface ISaveFileDialog
{
    Task<string?> ShowSaveFileDialogAsync(Dictionary<string, List<string>> fileTypes);
}
