using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IEndDragHandler, IDragHandler, IPointerEnterHandler, IBeginDragHandler
{
    [Header("Reference sprites")] 
    [SerializeField] private Sprite _nullReferenceSprite;
    
    [Header("Reference")]
    [SerializeField] private Image _icon;
    [SerializeField] private Animator _animator;

    [Header("VALUE")]
    [SerializeField] private GameObject _parentImageValue;
    [SerializeField] private TextMeshProUGUI _valueText;
    
    public int Count;
    [HideInInspector] public Item Item;

    #region Main

    private void Start()
        => ReloadVisual();

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
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var target in results)
        {
            if (target.gameObject.TryGetComponent(out ItemCell target_cell))
            {

                Item old_item = target_cell.Item;
                int old_value = target_cell.Count;

                if (Input.GetMouseButtonUp(1) && Count >= 2) // Move one item
                {
                    if (target_cell.Item)
                    {
                        if (target_cell.Item.TAG == Item.TAG && target_cell.Count + 1 <= target_cell.Item.Stack)
                        {
                            target_cell.SetCount(target_cell.Count + 1);
                            SetCount(Count - 1);
                        }
                    }
                    else
                    {
                        target_cell.SetItem(Item);
                        SetCount(Count - 1);
                    }
                }
                else // Move all item
                {
                    if (target_cell.Item && target_cell.Item.TAG == Item.TAG &&
                        target_cell.Count < target_cell.Item.Stack)
                    {
                        if (target_cell.Count + Count <= target_cell.Item.Stack)
                        {
                            target_cell.SetCount(target_cell.Count + Count);
                            DeleteItem();
                        }
                        else
                        {
                            int lack = target_cell.Item.Stack - target_cell.Count;
                            if (lack > 0 && Count >= lack)
                            {
                                target_cell.SetCount(target_cell.Count + lack);
                                SetCount(Count - lack);
                            }
                        }
                    }
                    else
                    {
                        target_cell.SetItem(Item, Count);
                        SetItem(old_item, old_value);
                    }
                }

                target_cell.ReloadVisual();
            }
        }
        
        _icon.transform.SetParent(transform);
        _icon.transform.position = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
        => _icon.transform.position = Camera.main.mousePosition();

    public void OnBeginDrag(PointerEventData eventData)
        => _icon.transform.SetParent(transform.parent.parent);
    
    public void OnPointerEnter(PointerEventData eventData)
        => _animator.Play("PointEnter");

    #endregion

    #region Visual

    public void ReloadVisual()
    {
        _icon.sprite = Item && Item.Icon ? Item.Icon : _nullReferenceSprite;
        _valueText.text = Count.ToString();
        _parentImageValue.SetActive(Count > 1);

        if(Count <= 0 && Item)
            DeleteItem();
    }

    #endregion
}