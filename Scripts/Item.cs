using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Item/New Item")]
public class Item : ScriptableObject
{
    [Header("Settings")]
    public string TAG;
    public string Title;
    [Multiline(3)] public string Description;
    public Sprite Icon;
    
    [Header("Stack")] 
    [Range(1, 64)] public int Stack = 1;
    
    [HideInInspector] public Inventory Inventory;
}
