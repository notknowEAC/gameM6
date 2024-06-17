using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPerfab;

    [SerializeField] private RectTransform contentPanel;
    
    [SerializeField] private UIInventoryDescription itemDescription;
    [SerializeField] private MouseFollower mouseFollower;
    List<UIInventoryItem> listOfUIItens = new List<UIInventoryItem>();
    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested,
        OnItemActionRequested,
        OnStartDrgging;

    public event Action<int, int> OnSwapItems;

    [SerializeField] private ItemActionPanel actionPanel;
    private void Awake()
    {
        Hide();
        mouseFollower.Toggle(false);
        itemDescription.ResetDescription();
    }
    public void InitalizeInventoryUI(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPerfab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfUIItens.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandledBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    internal void ResetAllItems()
    {
        foreach (var item in listOfUIItens)
        {
            item.ResetData();
            item.Deselect();
        }
    }
    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItems();
        listOfUIItens[itemIndex].Select();
    }
    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItens.Count > itemIndex)
        {
            listOfUIItens[itemIndex].SetData(itemImage, itemQuantity);
        }
    }
    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItens.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnItemActionRequested?.Invoke(index);
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItens.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        HandleItemSelection(inventoryItemUI);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandledBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItens.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDrgging?.Invoke(index);
    }

    public void CreateDrggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }
    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
       int index = listOfUIItens.IndexOf(inventoryItemUI);
       if (index == -1)
        return;
        OnDescriptionRequested?.Invoke(index);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        ResetSelection();
    }

    public void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    public void AddAction(string actionName, Action performAction)
    {
        actionPanel.AddButon(actionName, performAction);
    }
    public void ShowItemAction(int itemIndex)
    {

        actionPanel.Toggle(true);
        actionPanel.transform.position = listOfUIItens[itemIndex].transform.position;
    }
    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listOfUIItens)
        {
            item.Deselect();
        }
        actionPanel.Toggle(false);
    }
    public void Hide()
    {
        actionPanel.Toggle(false);
        gameObject.SetActive(false);
        ResetDraggedItem();
    }
}
}