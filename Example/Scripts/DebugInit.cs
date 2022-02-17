using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugInit : MonoBehaviour
{
    [SerializeField] private bool _isField;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ItemCell _cellPrefab;
    [SerializeField] private List<Item> _items = new List<Item>();

    private void Start()
        => Init();

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _inventory.Clear();
            Init();
        }
    }

    private void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            if (_isField)
            {
                Item item = _items.Random();
                _inventory.AddItem(item, Random.Range(1, item.Stack+1));
            }
        }
    }
}
