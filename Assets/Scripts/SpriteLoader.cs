using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    Dictionary<string, Vector2[]> tileUVMap;
    public static SpriteLoader instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        tileUVMap = new Dictionary<string, Vector2[]>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Subfolder");
        float imageWidth = 0f;
        float imageHeight = 0f;
        foreach(Sprite s in sprites)
        {
            if(s.rect.x + s.rect.width > imageWidth)
            {
                imageWidth = s.rect.x + s.rect.width;
            }
            if (s.rect.y + s.rect.height > imageHeight)
            {
                imageHeight = s.rect.y + s.rect.height;
            }
        }
        foreach(Sprite s in sprites)
        {
            Vector2[] uvs = new Vector2[4];
            uvs[0] = new Vector2(s.rect.x / imageWidth, s.rect.y / imageHeight);
            uvs[1] = new Vector2((s.rect.x + s.rect.width) / imageWidth, s.rect.y / imageHeight);
            uvs[2] = new Vector2(s.rect.x / imageWidth, (s.rect.y + s.rect.height) / imageHeight);
            uvs[3] = new Vector2((s.rect.x + s.rect.width) / imageWidth, (s.rect.y + s.rect.height) / imageHeight);
            tileUVMap.Add(s.name, uvs);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2[] GetTileUVs(Tile tile)
    {
        if (tile != null)
        {
            string key = tile.type.ToString();
            if (tileUVMap.ContainsKey(key) == true)
            {
                return tileUVMap[key];
            }
            else
            {
                Debug.Log("ERROR");
                return tileUVMap["Void"];
            }
        }
        else
        {
            return tileUVMap["Void"];
        }
    }

}
