using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField, Min(1)] private int _weight = 1;
    [SerializeField, Min(1)] private int _height = 1;
    [SerializeField] private ItemCell _itemCellPrefab;

    [Header("Cell")] 
    [SerializeField, Min(.01f)] private float _weightCell = 1;
    [SerializeField, Min(.01f)] private float _heightCell = 1;
    [SerializeField, Min(0)] private float _indent = 3;

    [Header("Settings")] 
    [SerializeField] public List<Category> _categories = new List<Category>();

    [HideInInspector] public UnityEvent OnChangeItems;
    [HideInInspector] public EventItemCell OnClickCell;
    [HideInInspector] public EventItemCell OnEnterCell;

    public ItemCell[,] _data { get; private set; }
    private Camera _cameraMain;

    #region Main

    private void Awake()
    {
        _cameraMain = Camera.main;
        InitCell();
    }

    private void InitCell()
    {
        _data = new ItemCell[_weight, _height];
        float sizeCell = GetSizeCell();

        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _weight; x++)
        {
            ItemCell itemCell = Instantiate(_itemCellPrefab, GetWorldPosition(new Vector3(x, y)), Quaternion.identity, transform);
            itemCell.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeCell, sizeCell);
            itemCell.Inventory = this;
            _data[x, y] = itemCell;
        }
    }
    
    #endregion

    #region InteractionsItem

    public void SwapItemPlace(ItemCell fromCell, ItemCell whereCell)
    {
        if (!fromCell.Item || !whereCell.Item) return;
        if(!fromCell.Item.HasContainsCategories(whereCell.Inventory._categories) || !whereCell.Item.HasContainsCategories(fromCell.Inventory._categories)) return;

        Item fromItem = fromCell.Item;
        int fromCount = fromCell.Count;

        fromCell.SetItem(whereCell.Item, whereCell.Count);
        whereCell.SetItem(fromItem, fromCount);
    }
    
    public void ItemDragCount(ItemCell fromCell, ItemCell whereCell, int count = 1)
    {
        if(fromCell.Count < count) return;
        if (whereCell.Item)
        {
            if (whereCell.Item.TAG == fromCell.Item.TAG && MetadataComparison(fromCell.Item, whereCell.Item) && whereCell.Count + count <= whereCell.Item.Stack)
            {
                whereCell.SetCount(whereCell.Count + count);
                fromCell.SetCount(fromCell.Count - count);
            }
            else SwapItemPlace(fromCell, whereCell);
        }
        else
        {
            whereCell.SetItem(fromCell.Item);
            fromCell.SetCount(fromCell.Count - count);
        }
    }
    
    public void ItemDragSplitter(ItemCell fromCell, ItemCell whereCell, float splitter = 1)
    {
        int halfCountOne = Mathf.RoundToInt(fromCell.Count * splitter);
        int halfCountTwo = fromCell.Count - halfCountOne;

        if (whereCell.Item)
        {
            if (whereCell.Item.TAG == fromCell.Item.TAG && MetadataComparison(fromCell.Item, whereCell.Item) && whereCell.Count + halfCountOne <= whereCell.Item.Stack)
            {
                whereCell.SetCount(whereCell.Count + halfCountOne);
                fromCell.SetCount(halfCountTwo);
            }
            else SwapItemPlace(fromCell, whereCell);
        }
        else
        {
            whereCell.SetItem(fromCell.Item, halfCountOne);
            fromCell.SetCount(halfCountTwo);
        }
    }
    
    public bool RemoveItem(Item targetItem, int count = 1, bool useMetaData = true)
    {
        if (count <= 0) 
            throw new IndexOutOfRangeException();
        if (GetCountItem(targetItem) < count)
            return false;

        int currentCount = count;
        List<ItemCell> itemCells = FindItems(targetItem, useMetaData);
        foreach (var itemCell in itemCells)
        {
            if (itemCell.Count <= currentCount)
            {
                currentCount -= itemCell.Count;
                itemCell.SetCount(0);
            }
            else if (itemCell.Count >= currentCount)
            {
                itemCell.SetCount(itemCell.Count - currentCount);
                currentCount = 0;
            }
        }
        
        OnChangeItems.Invoke();
        return true;
    }
    
    public void AddItem(Item target_item, int count = 1)
    {
        Item old_targetItem = target_item;
        target_item = Instantiate(old_targetItem);
        target_item.SetMetaDatas(PlasticLine.CloneDictionaryCloningValues(old_targetItem.GetMetaDatas())); 
        
        if (count <= 0) 
            throw new IndexOutOfRangeException();
        if (!target_item.HasContainsCategories(_categories))
            return;

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
    
    public ItemCell GetNullCell()
    {
        for (int y = _height-1; y >= 0; y--)
        for (int x = 0; x < _weight; x++)
            if (!_data[x, y].Item)
                return _data[x, y];
        return null;
    }
    
    public List<ItemCell> FindItems(Item target, bool useMetaData = true)
    {
        List<ItemCell> items = new List<ItemCell>();
        foreach (var item in _data)
            if (item.Item && item.Item.TAG == target.TAG)
                if(useMetaData)
                {
                    if(MetadataComparison(item.Item, target))
                        items.Add(item);
                }
                else items.Add(item);
        return items;
    }

    public bool MetadataComparison(Item oneMetaData, Item twoMetaData)
    {
        if (oneMetaData.GetMetaDatas().Count == twoMetaData.GetMetaDatas().Count)
        {
            foreach (var (key, value) in oneMetaData.GetMetaDatas())
                if (twoMetaData.GetMetaDatas().ContainsKey(key) && twoMetaData.GetMetaDatas().ContainsValue(value))
                {
                    
                }
                else return false;
        }
        else return false;

        return true;
    }
    
    public List<ItemCell> GetAllItems()
    {
        List<ItemCell> items = new List<ItemCell>();
        foreach (var item in _data)
            if (item.Item) items.Add(item);
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

    #region OtherFunctions

    private Vector2 GetWorldPosition(Vector2 gridPosition)
        => (Vector2) transform.position + new Vector2(gridPosition.x * _weightCell, gridPosition.y * _heightCell);
    
    public float GetSizeCell()
        => (_cameraMain.WorldToScreenPoint( GetWorldPosition(new Vector3(2, 1))).x - 
            _cameraMain.WorldToScreenPoint( GetWorldPosition(new Vector3(1, 1))).x) - _indent;

    public Vector2Int GetSzieGrid()
        => new Vector2Int(_weight, _height);
    
    #endregion
    
    #region Visual

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int y = 0; y < _height; y++)
            for (int x = 0; x < _weight; x++)
                Gizmos.DrawWireCube(GetWorldPosition(new Vector2(x, y)), new Vector3(_weightCell, _heightCell));
        }
    #endif

    #endregion
    
}

[Serializable] public class EventItemCell : UnityEvent<ItemCell> {}