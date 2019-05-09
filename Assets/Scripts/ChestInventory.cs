using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : MonoBehaviour
{
    private ChestUI ui;
    private int space = 16;
    public List<Item> items;
    public static ChestInventory instance;

    public CanvasGroup render;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //items = new List<Item>();
        ui = GetComponent<ChestUI>();
        render = GetComponent<CanvasGroup>();
        render.alpha = 0f;
        render.blocksRaycasts = false;
    }
    private void Start()
    {
    }
    public void SetItems(List<Item> itemsReference)
    {
        items = itemsReference;
        ui.UpdateUI();
    }
    public bool AddItem(Item item)
    {
        if (items.Count >= space)
        {
            return false;
        }

        items.Add(item);
        if (ui != null)
        {
            ui.UpdateUI();
        }
        else
        {
            return false;
        }
        return true;
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        ui.UpdateUI();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
