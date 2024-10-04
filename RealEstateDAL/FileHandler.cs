// Created by Pontus Åkerberg 2024-09-24
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Xml;

namespace RealEstateDAL;

/// <summary>
/// Class to handle the saving and opening of files.
/// </summary>
public class FileHandler
{
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
    /// Saves the given type T to a XML-file using DataContract and XMLWriter.
    /// </summary>
    /// <typeparam name="T">Type to serializ from.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <param name="objectToSerialize">Object to serialize and save</param>
    /// <returns>True if file created, false if not.</returns>
    static public bool SaveAsXML<T>(string filePath, T objectToSerialize)
    {
        try
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                NewLineOnAttributes = false,
            };

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    ser.WriteObject(xmlWriter, objectToSerialize);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Opens a XML-file with DataContract and deserialize it to the given typeparameter T.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to.</typeparam>
    /// <param name="filePath">Filepath to file</param>
    /// <returns>The given deserialized type, or default (null) if not.</returns>
    static public T? OpenXML<T>(string filePath)
    {
        try
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));

            using (StreamReader reader = new StreamReader(filePath))
            {
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    return (T?)ser.ReadObject(xmlReader);
                }
            }
        }
        catch
        {
            return default;
        }
    }
}
