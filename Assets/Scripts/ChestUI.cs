
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public Transform itemsParent;
    InventorySlot[] slots;
    ChestInventory inventory;
    // Start is called before the first frame update
    void Awake()
    {
        inventory = this.gameObject.GetComponent<ChestInventory>();//Equipment.instance.playerInventory;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
