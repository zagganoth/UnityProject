using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public Item item;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 clickPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            if(Mathf.FloorToInt(clickPos.x) == Mathf.FloorToInt(transform.position.x) && Mathf.FloorToInt(clickPos.y)== Mathf.FloorToInt(transform.position.y) && PlayerMovement.instance.InPlayerReach(clickPos))
            {
                if (Equipment.instance.playerInventory.AddItem(item))
                {

                    Destroy(gameObject);
                }

            }
        }
    }
}
