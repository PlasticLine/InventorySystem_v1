using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IEndDragHandler, IDragHandler, IPointerEnterHandler, IBeginDragHandler
{
    [Header("Reference sprites")] 
    [SerializeField] private GameObject _blockCellObject;
    [SerializeField] private Sprite _nullReferenceSprite;
    
    [Header("Reference")]
    [SerializeField] private Image _icon;
    [SerializeField] private Animator _animator;

    [Header("VALUE")]
    [SerializeField] private GameObject _parentImageValue;
    [SerializeField] private TextMeshProUGUI _valueText;
    
    [HideInInspector] public Item Item;
    [HideInInspector] public Inventory Inventory;
    [HideInInspector] public int Count { get; private set; }
    [HideInInspector] public bool _isBlock { get; private set; }

    private RectTransform _rectTransform;
    private SpriteRenderer _dragImage;

    #region Main

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        ReloadVisual();
    }

    #endregion

    #region InteractionsItem

    public void SetCount(int count = 1)
    {
        Count = count;
        ReloadVisual();
    }
    
    public void SetItem(Item targetItem, int count = 1)
    {
        Item = targetItem;
        Count = count;
        ReloadVisual();
    }
    
    public void SetBlock(bool flag)
    {
        _isBlock = flag;
        ReloadVisual();
    }
    
    public void DeleteItem()
    {
        Item = null;
        Count = 0;
        ReloadVisual();
    }
    
    #endregion
    
    #region Events

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!_dragImage) return;
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if(!_isBlock)
            foreach (var target in results)
                if (target.gameObject.TryGetComponent(out ItemCell target_cell) && target_cell != this && !target_cell._isBlock && Item.HasContainsCategories(target_cell.Inventory._categories))
                {
                    if (Input.GetMouseButtonUp(1)) // Move one item
                        Inventory.ItemDragCount(this, target_cell, 1);
                    else if (Input.GetMouseButtonUp(2)) // Move half item
                        Inventory.ItemDragSplitter(this,target_cell, 0.5f);
                    else // Move all item
                        Inventory.ItemDragSplitter(this, target_cell);
                    target_cell.ReloadVisual();
                }
        Destroy(_dragImage.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_dragImage)
            _dragImage.transform.position = Camera.main.mousePosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_dragImage && !_isBlock && Item)
        {
            _dragImage = new GameObject("DRAG ICON", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            _dragImage.sprite = Item.Icon;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
        => _animator.Play("PointEnter");

    #endregion

    #region Visual

    public void ReloadVisual()
    {
        _blockCellObject.SetActive(_isBlock);
        _icon.sprite = Item && Item.Icon ? Item.Icon : _nullReferenceSprite;
        _valueText.text = $"x{Count.ToString()}";
        _parentImageValue.SetActive(Count > 1);

        if(Count <= 0 && Item)
            DeleteItem();
    }

    #endregion
}