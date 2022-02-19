using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugInit : MonoBehaviour
{
    [SerializeField] private bool _isRemove;
    [SerializeField] private bool _isField;
    [SerializeField] private bool _isBlockRandom;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private List<Item> _items = new List<Item>();

    private void Start()
        => Init();

    private void Update()
    {
        if (Input.GetMouseButtonDown(3))
        {
            Vector2Int sizeGrid = _inventory.GetSzieGrid();
            
            for (int y = 0; y < sizeGrid.y; y++)
            for (int x = 0; x < sizeGrid.x; x++)
                _inventory._data[x, y].SetBlock(false);
            
            _inventory.Clear();
            Init();
        }

        if (Input.GetMouseButtonDown(4) && _isRemove)
        {
            Item item = _items.Random();
            int countRemove = Random.Range(1, item.Stack + 1);
            Debug.Log($"Remove: x{countRemove} {item.Title} | Status: {_inventory.RemoveItem(item, countRemove)}");
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
                if (Random.value < .5f && _isBlockRandom)
                {
                    Vector2Int sizeGrid = _inventory.GetSzieGrid();
                    Vector2Int index = new Vector2Int(Random.Range(0, sizeGrid.x), Random.Range(0, sizeGrid.y));
                    _inventory._data[index.x, index.y].SetBlock(true);
                }
            }
        }
    }
}
