using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Handheld handheld = null;
    [SerializeField] public InventoryManager playerInventory = null;
    public static Equipment instance;
    [SerializeField] SeedGrowth seedPrefab;
    EquipmentSlot[] slots;
    private void Awake()
    {
        instance = this;
        handheld = null;
        slots = GetComponentsInChildren<EquipmentSlot>();
    }
    /*
    public bool SetItem(EquipmentSlot slot)
    {
        Item curItem = InventoryManager.instance.dragUI.item;
        if(curItem.getEquipType() == Item.EquipType.Sword)
        {
            return true;
        }
        return true;
    }*/
    // Start is called before the first frame update
    public int getReach()
    {
        return handheld == null ? 2 : handheld.getReach();
    }
    void Start()
    {
        
    }
    public bool HandheldEmpty()
    {
        return handheld == null;
    }
    public void SetActiveHandheld(Handheld item)
    {
        handheld = item;
        
    }
    public void RemoveActiveHandheld()
    {
        if (handheld != null)
        {
            Destroy(handheld.gameObject);
            handheld = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonDown("Fire1") )
        {

            if (handheld != null)
            {
                handheld.gameObject.SetActive(true);
                handheld.WorldClicked(clickPosition.x, clickPosition.y);
            }
            float playerX = PlayerMovement.instance.transform.position.x;
            float playerY = PlayerMovement.instance.transform.position.y;
            int clickX = Mathf.FloorToInt(clickPosition.x);
            int clickY = Mathf.FloorToInt(clickPosition.y);
            if (System.Math.Abs(clickPosition.x - playerX) <= 2 && Mathf.FloorToInt(Mathf.Abs(clickPosition.y - playerY)) <= 2)
            {
                World.instance.WorldChangeClick(clickX, clickY,World.WorldClickType.Break);
            }
            if (InventoryManager.instance.selectSlot >= 0 && InventoryManager.instance.items[InventoryManager.instance.selectSlot].getUseType() == Item.UseType.Plant && World.instance.GetTileAt(clickX, clickY).type == Tile.Type.Till)
            {
                SeedGrowth seed = Instantiate(seedPrefab, new Vector2(clickX+0.5f, clickY+0.5f), Quaternion.identity);
                seed.setPlanted();
            }
        }*/
    }
}
