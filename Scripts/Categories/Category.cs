using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/New Category")]
public class Category : ScriptableObject
{
    [Header("Settings")] 
    public string TAG;
    public string Title;
    [Multiline(3)] public string Description;
}
