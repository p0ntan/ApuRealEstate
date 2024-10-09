// Created by Pontus Åkerberg 2024-09-24
using System.Runtime.Serialization;

namespace RealEstateBLL.Manager;

/// <summary>
/// Dictionary manager is a generic manager class to manage any type both as value and key.
/// </summary>
/// <typeparam name="TKey">Type of key</typeparam>
/// <typeparam name="TValue">Type of value</typeparam>
[DataContract]
public class DictionaryManager<TKey, TValue> : IDictionaryManager<TKey, TValue> where TKey : notnull
{
    /// <summary>
    /// Dictionary to keep all values.
    /// SerializableDictionary is just a normal Dictionary with added contrat attributes.
    /// </summary>
    [DataMember(Name = "Dictionary")]
    private SerializableDictionary<TKey, TValue> _dictionary;

    /// <summary>
    /// Number of items in dictionary.
    /// </summary>
    public int Count {
        get { return _dictionary.Count; }
    }

    /// <summary>
    /// Protected access to dictionary in manager.
    /// </summary>
    protected Dictionary<TKey, TValue> Dictionary { get { return _dictionary; } }

    // Default constructor
    public DictionaryManager()
    {
        _dictionary = new SerializableDictionary<TKey, TValue>();
    }

    /// <summary>
    /// Adds an item to the dictionary with the given key. Checks that the key isn't already in dictionary and that the item is not null.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <returns>True if added, false if not.</returns>
    public virtual bool Add(TKey key, TValue item)
    {
        bool itemAdded = false;

        if (item != null && !CheckKey(key))
        {
            _dictionary.Add(key, item);
            itemAdded = true;
        }

        return itemAdded;
    }

    /// <summary>
    /// Gets an item of a certain key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Item if exitsts, defaul (null) if not.</returns>
    public TValue? Get(TKey key)
    {
        TValue? item = default;

        if (CheckKey(key))
            item = _dictionary[key];

        return item;
    }

    /// <summary>
    /// Changes an item in the dictionary at the given key. Checks that the key exists dictionary and that the item is not null.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <returns>True if changed, false if not.</returns>
    public bool Change(TKey key, TValue item)
    {
        bool itemChanged = false;

        if (item != null && CheckKey(key))
        {
            _dictionary[key] = item;
            itemChanged = true;
        }

        return itemChanged;
    }

    /// <summary>
    /// Checks that a gíven key is in the manager.
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True if key exists, false if not.</returns>

    public bool CheckKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    /// <summary>
    /// Deletes all items in the dictionary.
    /// </summary>
    public void DeleteAll()
    {
        _dictionary = new SerializableDictionary<TKey, TValue>();
    }

    /// <summary>
    /// Deletes an item at a given key. Validates the key by checking that it exsists first.
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True if deleted, false if not.</returns>
    public bool Delete(TKey key)
    {
        bool itemRemoved = false;

        if (CheckKey(key))
        {
            _dictionary.Remove(key);
            itemRemoved = true;
        }

        return itemRemoved;
    }

    /// <summary>
    /// Loops through the dictionary and returns an array with each item as a string with ToString method.
    /// </summary>
    /// <returns>Array of items as strings.</returns>
    public string[] ToStringArray()
    {
        IEnumerable<TValue> sortedValues = _dictionary.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);

        string[] strings = new string[_dictionary.Count];
        int index = 0;

        foreach (TValue itemValue in sortedValues)
        {
            strings[index] = itemValue?.ToString() ?? string.Empty;
            index++;
        }

        return strings;
    }

    /// <summary>
    /// Loops through the dictionary and returns a dictionary with the key as key and the string representation as value.
    /// </summary>
    /// <returns>Dictionary of items with key and string representation.</returns>
    public Dictionary<TKey, string> ToStringDictionary()
    {
        Dictionary<TKey, string> asDict = new();

        foreach (KeyValuePair<TKey, TValue> item in _dictionary)
        {
            asDict.Add(item.Key, item.Value?.ToString() ?? string.Empty);
        }

        return asDict;
    }
}
