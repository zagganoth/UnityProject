using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Item item;
    public Image slotImage;
    [SerializeField] Sprite SelectedSprite;
    [SerializeField] Sprite DisabledSprite;
    public Sprite defaultSprite;
    [SerializeField] bool isChestSlot = false;
    public TextMeshProUGUI text;
    public int slotNum;
    public bool slotEnabled;
    private void Awake()
    {
        slotNum = -1;
        slotImage = GetComponent<Image>();
        defaultSprite = slotImage.sprite;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.gameObject.SetActive(false);
        item = null;
        slotEnabled = true;
    }
    public bool hasItem()
    { 
        return item != null;
    }
    public void AddItem(Item newItem)
    {
        if (item == null)
        {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        if (item.stackable)
        {
            RefreshCount();
        }
    }
    public void RefreshCount()
    {
        text.gameObject.SetActive(true);
        text.text = item.count.ToString();
    }
    public Item getItem()
    {
        return item;
    }
    public void SetDisabled()
    {
        slotImage.sprite = DisabledSprite;
        slotEnabled = false;
    }
    private void NormalUseItem()
    {
        if (item != null)
        {
            if (item.Use())
            {  
                if (slotNum != InventoryManager.instance.selectSlot)
                {
                    slotImage.sprite = SelectedSprite;
                    InventoryManager.instance.selectSlot = slotNum;
                }
                else
                {
                    SetInactiveSlot();
                }
                //InventoryManager.instance.onItemChangedCallback.Invoke();
            }
            else if (slotNum == InventoryManager.instance.equipSlot)
            {
                SetInactiveSlot();
            }
        }
    }
    private void ChestOrNormalUse()
    {
        if (!isChestSlot)
        {
            NormalUseItem();
        }
        else
        {

            ChestUseItem();
        }
    }
    public void UseItem()
    {
        if (slotEnabled)
        {
            if (InventoryManager.instance.dragging)
            {
                if (hasItem() == false)
                {
                    //ChestOrNormalUse();
                    AddItem(InventoryManager.instance.dragUI.item);
                    InventoryManager.instance.AddItem(item, slotNum);
                    InventoryManager.instance.dragUI.Placed();
                }
            }
            else if (item != null && InventoryManager.instance.dragMode && !isChestSlot)
            {

                DraggableObject drag = InventoryManager.instance.dragUI;
                drag.BeginDrag(item);
                /*
                Image dragImage = drag.GetComponent<Image>();
                dragImage.sprite = item.icon;
                dragImage.color = new Color(dragImage.color.r, dragImage.color.g, dragImage.color.b, 1f);
                drag.item = item;*/
                InventoryManager.instance.RemoveItem(slotNum);
                SetInactiveSlot();
                ClearSlot();

            }
            else
            {
                ChestOrNormalUse();
            }
        }
    }
    private void ChestUseItem()
    {
        if (item != null && item.transferToInventory())
        {
            ClearSlot();
        }
    }
    public void SetActiveSlot()
    {
        if (item != null && item.getUseType() == Item.UseType.Select)
        {

            item.Use();

        }
        slotImage.sprite = SelectedSprite;
    }
    public bool isActiveSlot()
    {
        return (slotImage.sprite == SelectedSprite);
    }
    public void SetInactiveSlot()
    {

        //if (InventoryManager.instance.selectSlot == slotNum) InventoryManager.instance.selectSlot = -1;
        slotImage.sprite = defaultSprite;
    }
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        text.gameObject.SetActive(false);
    }
}
