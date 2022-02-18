using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugInit : MonoBehaviour
{
    [SerializeField] private bool _isField;
    [SerializeField] private bool _isBlockRandom;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ItemCell _cellPrefab;
    [SerializeField] private List<Item> _items = new List<Item>();

    private void Start()
        => Init();

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector2Int sizeGrid = _inventory.GetSzieGrid();
            
            for (int y = 0; y < sizeGrid.y; y++)
            for (int x = 0; x < sizeGrid.x; x++)
                _inventory._data[x, y].SetBlock(false);
            
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
                if (Random.value < .5f && _isBlockRandom)
                {
                    Vector2Int index = new Vector2Int(Random.Range(0, _inventory.Weight), Random.Range(0, _inventory.Height));
                    _inventory._data[index.x, index.y].SetBlock(true);
                }
            }
        }
    }
}
