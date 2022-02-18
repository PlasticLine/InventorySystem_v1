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

    [HideInInspector] public Inventory Inventory;
}