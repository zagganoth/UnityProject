
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public enum UseType { Equip,Select, Plant};
    //[SerializeField] protected bool isEquipment;
    [SerializeField] public Handheld equipment;
    [SerializeField] protected UseType type;
    public enum EquipType { Head, Chest, Pant, Sword, None};
    [SerializeField] EquipType equipType;
    private Animator animator;
    [SerializeField] public bool stackable;
    public int count;
    public virtual bool Use()
    {
        return true;
    }
    public virtual UseType getUseType()
    {
        return type;
    }
    public virtual EquipType getEquipType()
    {
        return equipType;
    }
    public virtual void SetInactive()
    {
        /*
        if(isEquipment)
        {
            Equipment.instance.RemoveActiveHandheld();
        }*/
        //InventoryManager.instance.selectSlot = -1;
    }
    public virtual bool transferToInventory()
    {
        if (Equipment.instance.playerInventory.AddItem(this))
        {
            ChestInventory.instance.RemoveItem(this);
            return true;
        }
        return false;
    }
}
