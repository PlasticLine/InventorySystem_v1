using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class PlasticLine
{
    public static T Random<T>(this List<T> items)
        => items[UnityEngine.Random.Range(0, items.Count)];
    
    public static Vector2Int ConvertInt(this Vector2 vector)
        => new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    
    public static Vector2 centerPosition(this Camera camera)
    {
        Vector2 center_screen = new Vector2(Screen.width, Screen.height)/2;
        return camera.ScreenToWorldPoint(center_screen);
    }

    public static Vector2 mousePosition(this Camera camera)
        => camera.ScreenToWorldPoint(Input.mousePosition);
    
    public static void AddEvent(this EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry(){ eventID = type };
        entry.callback.AddListener(action.Invoke);
        trigger.triggers.Add(entry);
    }
    
    public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
        (Dictionary<TKey, TValue> original) where TValue : ICloneable
    {
        Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
            original.Comparer);
        foreach (KeyValuePair<TKey, TValue> entry in original)
            ret.Add(entry.Key, (TValue) entry.Value.Clone());
        return ret;
    }
}
