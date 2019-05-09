using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Item[] items;
    private int space = 21;
    private int hotbarLength = 7;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int equipSlot;
    public int selectSlot;
    public Transform itemsParent;
    InventorySlot[] slots;
    float prevScroll;
    public bool dragMode;
    [SerializeField] public DraggableObject dragUI;
    private List<Item> seenStackableItems;
    public bool dragging;
    // Start is called before the first frame update
    void Awake()
    {
        prevScroll = 0f;
        equipSlot = -1;
        selectSlot = -1;
        items = new Item[space];
        instance = this;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].slotNum == -1) slots[i].slotNum = i;
            if (i >= space) slots[i].SetDisabled();
        }
        dragMode = true;
        dragging = false;
        seenStackableItems = new List<Item>();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        float scrollVal = Input.GetAxis("Mouse ScrollWheel");
        if (scrollVal > 0f && selectSlot < hotbarLength - 1)
        {
            selectSlot += 1;
            slots[selectSlot].SetActiveSlot();
            UpdateUI();
        }
        else if(scrollVal < 0f && selectSlot > 0)
        {
            selectSlot -= 1;
            slots[selectSlot].SetActiveSlot();
            UpdateUI();
        }
        prevScroll = scrollVal;
        if(Input.GetButtonDown("Cancel"))
        {
            dragMode = !dragMode;
        }
    }
    public bool PlantableSelected()
    {
        bool plantable =  (selectSlot >= 0 && items[selectSlot] != null && items[selectSlot].count > 0 && InventoryManager.instance.items[InventoryManager.instance.selectSlot].getUseType() == Item.UseType.Plant);

        return plantable;
    }
    public void ReduceSelectedItem()
    {
        bool plantable = PlantableSelected();
        if (plantable)
        {
            items[selectSlot].count -= 1;
            slots[selectSlot].RefreshCount();
        }
    }
    public virtual bool AddItem(Item item, int slotNum=-1)
    {
        if(slotNum == -1)
        {
            if(!seenStackableItems.Contains(item))
            {
                Debug.Log("Adding new items to seen list");
                item.count = 1;
                seenStackableItems.Add(item);
            }
            int index = Array.IndexOf(items, item);
            if (index == -1 || !item.stackable)
            {
                for (int i = 0; i < space; i++)
                {

                    if (!slots[i].hasItem())
                    {
                        Debug.Log("Adding item to slot " + i);
                        slots[i].AddItem(item);
                        items[i] = item;
                        break;
                    }
                }
            }
            else
            {
                items[index].count++;
                slots[index].AddItem(item);
            }
        }
        else
        {
            items[slotNum] = item;
        }
        /*
        if(items.Length >= space)
        {
            return false;
        }
        if (item.stackable && items.Contains(item))
        {
            items[items.IndexOf(item)].count += 1;
        }
        else
        {
            if (item.count == 0) item.count = 1;
            items.Add(item);
        }
        UpdateUI();
        return true;*/
        return true;
    }
    public void RemoveItem(int slotNum)
    {

        items[slotNum] = null;
        UpdateUI();
    }
    public void UpdateUI()
    {

        
        bool hasActiveSlot = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Length)
            {
                if (slots[i].slotNum != InventoryManager.instance.selectSlot && slots[i].isActiveSlot())
                {
                    slots[i].SetInactiveSlot();
                }
                if (slots[i].isActiveSlot()) hasActiveSlot = true;
            }
        }
        if (!hasActiveSlot) InventoryManager.instance.selectSlot = -1;
        
    }
}
