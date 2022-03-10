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