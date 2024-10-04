// Created by Pontus Åkerberg 2024-09-24

namespace RealEstateBLL.Manager;

/// <summary>
/// Interface that should be implemented by all DictionaryManagers
/// </summary>
/// <typeparam name="TKey">Type of key</typeparam>
/// <typeparam name="TValue">Type of value</typeparam>
public interface IDictionaryManager<TKey, TValue>
{
    /// <summary>
    /// Gets the number of items in manager
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Add an item to the manager.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <returns>True if added, false if not.</returns>
    bool Add(TKey key, TValue item);

    /// <summary>
    /// Change the item at a certain key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <returns>True if changed, false if not.</returns>
    bool Change(TKey key, TValue item);

    /// <summary>
    /// Controls that a certain key exists in the manager.
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True if key exists, false if not.</returns>
    bool CheckKey(TKey key);

    /// <summary>
    /// Deletes all entires in manager.
    /// </summary>
    void DeleteAll();

    /// <summary>
    /// Deletes the item with the given key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True if deleted, false if not.</returns>
    bool Delete(TKey key);

    /// <summary>
    /// Get a certain item with a given key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Returns item if key exist, if not default (null)</returns>
    TValue? Get(TKey key);

    /// <summary>
    /// Get the all the items in the manager as an array of string representations.
    /// </summary>
    /// <returns>Array of strings representing each item in manager.</returns>
    string[] ToStringArray();
}
