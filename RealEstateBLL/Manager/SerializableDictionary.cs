// Created by Pontus Åkerberg 2024-09-24
using System.Runtime.Serialization;

namespace RealEstateBLL.Manager;

/// <summary>
/// SerializeableDictionary is just a regular dictionary with added attribute [CollectionDataContract]
/// for better printout when serializing to XML.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>

[CollectionDataContract(Name = "SerializableDictionary", ItemName = "Item", KeyName = "Key", ValueName = "Value")]
public sealed class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{ }
