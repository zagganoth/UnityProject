using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] bool isLootChest;
    [SerializeField] Item[] lootPool;
    private Transform canvas;
    private ChestInventory inventory = null;
    [SerializeField] Sprite openSprite;
    private Sprite defaultSprite;
    private SpriteRenderer render;
    //[SerializeField] ChestInventory invPrefab;
    private bool lootGenerated = false;
    List<Item> items;
    private bool open = false;
    private void Awake()
    {

        items = new List<Item>();
        render = GetComponent<SpriteRenderer>();
        defaultSprite = render.sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        canvas = CanvasInstance.instance.transform;
        /*if (inventory == null)
        {
            inventory = Instantiate(invPrefab);
            inventory.transform.SetParent(canvas.transform);
            inventory.transform.position = new Vector3(230, 150, 0);
            inventory.transform.localScale = new Vector3(1, 1);

        }*/
        inventory = ChestInventory.instance;


    }
    void generateLootFromPool()
    {
        if(lootPool.Length > 0)
        {
            int lootIndex = Random.Range(0, 1000) % lootPool.Length;
            inventory.SetItems(items);
            if (inventory.AddItem(lootPool[lootIndex]))
            {
                lootGenerated = true;
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isLootChest && !lootGenerated)
        {

            generateLootFromPool();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            //Vector2 clickPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(hit && hit.transform.position == this.transform.position && PlayerMovement.instance.InPlayerReach(transform.position))
            {
                if (!open)
                {
                    OpenChest();
                    
                }
                else
                {
                    CloseChest();
                }
            }
        }
        if(open && !PlayerMovement.instance.InPlayerReach(transform.position))
        {
            CloseChest();
        }
    }
    void OpenChest()
    {
        render.sprite = openSprite;
        inventory.SetItems(items);
        ChestInventory.instance.render.alpha = 1f;
        ChestInventory.instance.render.blocksRaycasts = true;
        open = true;
    }
    void CloseChest()
    {
        open = false;
        render.sprite = defaultSprite;
        ChestInventory.instance.render.alpha = 0f;
        ChestInventory.instance.render.blocksRaycasts = false;
    }
}
