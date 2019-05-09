using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBreak : MonoBehaviour
{
    [SerializeField] ItemPickup woodPrefab;
    [SerializeField] Sprite[] breakStages;
    private int hits;
    private int origHits;
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        hits = (Random.Range(0, 1000) % 5) + 1;
        origHits = hits;
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnWood()
    {
        int numWood = (Random.Range(0, 1000) % 7) + 3;

        for(int i = 0; i < numWood; i++)
        {

            //Instantiate(woodPrefab, new Vector2(x + xAdd, y + yAdd), Quaternion.identity);
            InventoryManager.instance.AddItem(woodPrefab.item);
        }
        hits -= 1;
        render.sprite = breakStages[(origHits-1)- hits];
        if(hits <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
