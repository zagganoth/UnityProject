
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    InventorySlot[] slots;
    InventoryManager inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = this.gameObject.GetComponent<InventoryManager>();//Equipment.instance.playerInventory;
        //inventory.onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateUI()
    {
        /*
        bool hasActiveSlot = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                if (slots[i].slotNum == -1) slots[i].slotNum = i;
                else if (slots[i].slotNum != InventoryManager.instance.selectSlot && slots[i].isActiveSlot())
                {
                    slots[i].SetInactiveSlot();
                }
                if (slots[i].isActiveSlot()) hasActiveSlot = true;
                slots[i].AddItem(inventory.items[i]);

            }
            else
            {
                slots[i].ClearSlot();
                if(slots[i].slotNum==-1)slots[i].slotNum = i;
            }
        }
        if (!hasActiveSlot) InventoryManager.instance.selectSlot = -1;*/
    }
}
