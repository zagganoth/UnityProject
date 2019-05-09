using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGrowth : MonoBehaviour
{
    bool planted = false;
    [SerializeField] Sprite[] growthStages;
    SpriteRenderer render;
    Sprite defaultSprite;
    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        defaultSprite = render.sprite;
    }
    public void setPlanted()
    {
        planted = true;
        render.sprite = growthStages[0];
        StartCoroutine(Growth());
    }
    IEnumerator Growth()
    {
        int stage = 0;
        float growthTime = 1f;
        while(stage < growthStages.Length - 1)
        {

            yield return new WaitForSeconds(growthTime);
            growthTime *= 2;
            render.sprite = growthStages[++stage];
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
