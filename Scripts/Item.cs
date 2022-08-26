using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Item/New Item")]
public class Item : ScriptableObject
{
    [Header("Settings")]
    public string TAG;
    
    [Header("UI")]
    public string Title;
    [Multiline(2)] public string Description;
    public Sprite Icon;
    
    [Header("Stack")]
    [Range(1, 64)] public int Stack = 1;

    [Header("Categories")] 
    public List<Category> Categories = new List<Category>();

    [HideInInspector] public Inventory Inventory;
    private Dictionary<string, string> _metaData = new Dictionary<string, string>();

    public string GetMetaData(string key)
        => _metaData.TryGetValue(key, out string value) ? value : null;

    public void SetMetaData(string key, string value)
    {
        if(!_metaData.ContainsKey(key)) 
            _metaData.Add(key, value);
    }

    public void SetMetaDatas(Dictionary<string, string> metaData)
        => _metaData = metaData;
    
    public Dictionary<string, string> GetMetaDatas()
        => _metaData;

    public bool HasContainsCategories(List<Category> targetCategories)
    {
        if (targetCategories == null)
            return false;

        bool[] listEnable = new bool[targetCategories.Count];
        for (int index = 0; index < listEnable.Length; index++)
        {
            Category targetCategory = targetCategories[index];
            foreach (var category in Categories)
                if (targetCategory.TAG == category.TAG)
                    listEnable[index] = true;
        }

        foreach (var enable in listEnable)
            if (!enable)
                return false;
        
        return true;
    }
}