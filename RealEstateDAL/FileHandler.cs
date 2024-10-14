// Created by Pontus Åkerberg 2024-09-24
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace RealEstateDAL;

/// <summary>
/// Class to handle the saving and opening of files.
/// </summary>
public class FileHandler
{
    /// <summary>
    /// Method to use for saving file, chooses the right filetype based on fileextension.
    /// </summary>
    /// <typeparam name="T">Type to serialize from.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <param name="objectToSerialize">Object to serialize and save</param>
    /// <returns>True if file saved, false if not.</returns>
    static public bool SaveToFile<T>(string filePath, T objectToSerialize)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        switch (fileExtension)
        {
            case ".json":
                return SaveAsJson<T>(filePath, objectToSerialize);
            case ".xml":
                return SaveAsXML<T>(filePath, objectToSerialize);
            default:
                return false;
        }
    }

    /// <summary>
    /// Method to use for opening a file, chooses the right filetype based on fileextension.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <returns>Given types if loaded correctly, null if not..</returns>
    static public T? OpenFile<T>(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        switch (fileExtension)
        {
            case ".json":
                return OpenJson<T>(filePath);
            case ".xml":
                return OpenXML<T>(filePath);
            default:
                return default;
        }
    }

    /// <summary>
    /// Saves the given type T to a JSON-file.
    /// </summary>
    /// <typeparam name="T">Type to serialize from.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <param name="objectToSerialize">Object to serialize and save</param>
    /// <returns>True if file created, false if not.</returns>
    static public bool SaveAsJson<T>(string filePath, T objectToSerialize)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                var options = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };

                string jsonString = JsonConvert.SerializeObject(objectToSerialize, options);

                writer.Write(jsonString);
            }

            return true;

        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Opens a JSON-file and deserialize it to the given typeparameter T.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <returns>The given deserialized type, or default (null) if not.</returns>
    static public T? OpenJson<T>(string filePath)
    {
        try
        {
            T? type = default;

            using (StreamReader reader = new StreamReader(filePath))
            {
                var options = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                string jsonString = reader.ReadToEnd();
                type = JsonConvert.DeserializeObject<T>(jsonString, options);
            }

            return type;
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Saves the given type T to a XML-file using XMLSerializer.
    /// </summary>
    /// <typeparam name="T">Type to serializ from.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <param name="objectToSerialize">Object to serialize and save</param>
    /// <returns>True if file created, false if not.</returns>
    static public bool SaveAsXML<T>(string filePath, T objectToSerialize)
    {
        try
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                ser.Serialize(writer, objectToSerialize);
                writer.Close();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Opens a XML-file with XMLSerializer and deserialize it to the given typeparameter T.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <returns>The given deserialized type, or default (null) if not.</returns>
    static public T? OpenXML<T>(string filePath)
    {
        try
        {
            T? type = default;

            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (Stream reader = new FileStream(filePath, FileMode.Open))
            {
                type = (T?)ser.Deserialize(reader);
                reader.Close();
            }

            return type;
        }
        catch
        {
            return default;
        }
    }
}
