using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugInit : MonoBehaviour
{
    [SerializeField] private bool _isRemove;
    [SerializeField] private bool _isField;
    [SerializeField] private bool _isBlockRandom;
    [SerializeField] private bool _isMetaDataRandom;
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
        
        if(_isMetaDataRandom)
            foreach (var itemCell in _inventory.GetAllItems())
            {
                if(itemCell.Item)
                    if (Random.value < 0.5f)
                        itemCell.Item.SetMetaData("_strength", Random.Range(0, 101).ToString());
            }
    }

    private void OnGUI()
    {
        if(!_isMetaDataRandom) return;
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.normal.background = Texture(new Color(1f, 1f, 1f));
        
        foreach (var itemCell in _inventory.GetAllItems())
        {
            Item item = itemCell.Item;
            if (item && item.HasMetaData("_strength"))
            {
                int strength = int.Parse(item.GetMetaData("_strength"));

                float inventorySize = _inventory.GetSizeCell();
                Vector3 screenPos = Camera.main.WorldToScreenPoint(itemCell.transform.position);
                Rect rectBackGround = new Rect(new Vector2(screenPos.x-inventorySize/2, Screen.height-screenPos.y+inventorySize/2), new Vector2(inventorySize, inventorySize/20));
                Rect rectProgress = new Rect(new Vector2(screenPos.x-inventorySize/2, Screen.height-screenPos.y+inventorySize/2), new Vector2(inventorySize / 100 * strength, inventorySize/20));
                
                GUI.backgroundColor = new Color(1f, 1f, 1f, 0.3f);
                GUI.Box(rectBackGround, String.Empty, guiStyle);
                
                GUI.backgroundColor = new Color(1f, 1f, 1f, 0.6f);
                GUI.Box(rectProgress, String.Empty, guiStyle);
            }
        }
    }

    private Texture2D Texture(Color32 color, int size = 1)
    {
        Texture2D texture2D = new Texture2D(size, size);
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
            texture2D.SetPixel(y, x, color);
        return texture2D;
    }
    
}
