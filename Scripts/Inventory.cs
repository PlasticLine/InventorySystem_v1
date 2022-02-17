using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [Header("Grid")]
    [Min(1)] public int Weight = 1;
    [Min(1)] public int Height = 1;
    [SerializeField] private ItemCell _itemCellPrefab;

    [HideInInspector] public UnityEvent OnChangeItems;
    
    private ItemCell[,] _data;
    
    #region Main

    private void Awake()
        => InitCell();
 
    private void InitCell()
    {
        _data = new ItemCell[Weight, Height];
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Weight; x++)
            _data[x, y] = Instantiate(_itemCellPrefab, GetWorldPosition(new Vector3(x, y)), Quaternion.identity, transform);
    }
    
    #endregion

    #region InteractionsItem

    public void AddItem(Item target_item, int count = 1)
    {
        if (count <= 0) 
            throw new IndexOutOfRangeException();

        int currentCount = count;
        do
        {
            // Find stack
            List<ItemCell> itemCells = FindItems(target_item);
            foreach (var itemCell in itemCells)
            {
                int lack = target_item.Stack - itemCell.Count;
                if(lack > 0)
                    if (currentCount >= lack)
                    {
                        itemCell.SetCount(itemCell.Count + lack);;
                        currentCount -= lack;
                    }
                    else
                    {
                        itemCell.SetCount(itemCell.Count + currentCount);
                        currentCount = 0;
                    }
            }

            // Create stack
            if (currentCount >= target_item.Stack)
            {
                ItemCell itemCell = GetNullCell();
                if (itemCell)
                    itemCell.SetItem(target_item, target_item.Stack);
                currentCount -= target_item.Stack;
            }
            else
            {
                ItemCell itemCell = GetNullCell();
                if (itemCell)
                    itemCell.SetItem(target_item, currentCount);
                currentCount = 0;
            }
            

        } while (currentCount > 0);
        OnChangeItems.Invoke();
    }
    
    private ItemCell GetNullCell()
    {
        for (int y = Height-1; y >= 0; y--)
        for (int x = 0; x < Weight; x++)
            if (!_data[x, y].Item)
                return _data[x, y];
        return null;
    }
    
    private List<ItemCell> FindItems(Item target)
    {
        List<ItemCell> items = new List<ItemCell>();
        foreach (var item in _data)
            if (item.Item && item.Item.TAG == target.TAG)
                items.Add(item);
        return items;
    }

    public int GetCountItem(Item target)
    {
        int current_count = 0;
        foreach (var item in _data)
            if (item.Item && item.Item.TAG == target.TAG)
                current_count += item.Count;
        return current_count;
    }
    
    public void Clear()
    {
        foreach (var itemCell in _data)
            itemCell.DeleteItem();
    }
    
    #endregion

    #region VisualDebug

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Weight; x++)
                Gizmos.DrawWireCube(GetWorldPosition(new Vector2(x, y)), Vector3.one);
        }
    #endif
    
    private Vector2 GetWorldPosition(Vector2 gridPosition)
        => (Vector2) transform.position + new Vector2(gridPosition.x, gridPosition.y);

    #endregion
    
}
