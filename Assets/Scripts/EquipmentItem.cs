using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment1 : Item
{

    public override bool Use()
    {

        if (Equipment.instance.HandheldEmpty())
        {
            type = UseType.Equip;
            Handheld instance = Instantiate(equipment, new Vector3(PlayerMovement.instance.transform.position.x, PlayerMovement.instance.transform.position.y - 0.5f, 0), Quaternion.identity);
            instance.transform.parent = PlayerMovement.instance.transform;
            instance.gameObject.SetActive(false);
            Equipment.instance.SetActiveHandheld(instance);
            return true;
        }
        return false;
    }
    public override void SetInactive()
    {
        //Equipment.instance.RemoveActiveHandheld();
        //InventoryManager.instance.equipSlot = -1;
    }
}
