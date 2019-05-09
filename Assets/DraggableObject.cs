using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableObject : MonoBehaviour
{
    RectTransform t;
    public Item item;
    Image dragImage;
    CanvasRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        dragImage = GetComponent<Image>();

        //dragImage.color = new Color(dragImage.color.r, dragImage.color.g, dragImage.color.b, 0f);
        t = GetComponent<RectTransform>();
        transform.SetAsLastSibling();
        render = GetComponent<CanvasRenderer>();
        render.SetAlpha(0f);
    }
    public void Placed()
    {

        item = null;
        //dragImage.color = new Color(dragImage.color.r, dragImage.color.g, dragImage.color.b, 0f);
        render.SetAlpha(0f);
        InventoryManager.instance.dragging = false;
    }
    public void BeginDrag(Item setItem)
    {
        item = setItem;
        dragImage.sprite = item.icon;
        //dragImage.color = new Color(dragImage.color.r, dragImage.color.g, dragImage.color.b, 1f);
        render.SetAlpha(1f);
        InventoryManager.instance.dragging = true;
    }
    // Update is called once per frame
    void Update()
    {
        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Input.mousePosition;
    }
}
