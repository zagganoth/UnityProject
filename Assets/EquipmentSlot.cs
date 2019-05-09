using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    Image image;
    Item item;
    Sprite defaultSprite;
    [SerializeField] Item.EquipType requiredEquipType;
    // Start is called before the first frame update
    void Start()
    {
        item = null;
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
    }
    public void UseItem()
    {
        if (item == null)
        {
            SetActiveItem();
        }
        else if(InventoryManager.instance.dragMode && InventoryManager.instance.dragging == false)
        {
            SetInactive();
        }
        else if(InventoryManager.instance.dragging)
        {
            SwapItem();
        }
    }
    void SetActiveItem()
    {
        if (InventoryManager.instance.dragUI.item.getEquipType() == requiredEquipType)
        {
            Item setItem = InventoryManager.instance.dragUI.item;
            image.sprite = setItem.icon;
            item = setItem;
            InventoryManager.instance.dragUI.Placed();
            SetEquipEffects(requiredEquipType, item);
           
        }
    }
    void SwapItem()
    {
        if(InventoryManager.instance.dragUI.item.getEquipType() == requiredEquipType)
        {
            Item setItem = InventoryManager.instance.dragUI.item;
            /*InventoryManager.instance.dragUI.item = item;*/
            InventoryManager.instance.dragUI.BeginDrag(item);
            image.sprite = setItem.icon;
            item = setItem;
            Equipment.instance.RemoveActiveHandheld();
            SetEquipEffects(requiredEquipType, item);

        }
    }
    static void SetEquipEffects(Item.EquipType equipType,Item item)
    {
        if(equipType == Item.EquipType.Sword)
        {

            item.Use();
        }
    }
    void SetInactive()
    {
        InventoryManager.instance.dragUI.BeginDrag(item);
        image.sprite = defaultSprite;
        item = null;
        Equipment.instance.RemoveActiveHandheld();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
