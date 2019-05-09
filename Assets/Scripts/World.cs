using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    public static World instance;
    public Tile[,] tiles;
    private Dictionary<Vector2Int,Tile[,]> tileMaps;
    [SerializeField] Material material;
    private Dictionary<Vector2Int,Mesh> meshDict;

    [SerializeField] int seed;
    [SerializeField] int oreSeed;
    [SerializeField] bool randomSeed;
    [SerializeField] float frequency;
    [SerializeField] float amplitude;

    [SerializeField] float lacunarity;
    [SerializeField] float persistence;

    [SerializeField] int octaves;

    [SerializeField] float seaLevel;

    [SerializeField] float shallowWaterHeight;
    [SerializeField] float shallowWaterEndHeight;

    [SerializeField] float beachStartHeight;
    [SerializeField] float beachEndHeight;

    [SerializeField] float grassStartHeight;
    [SerializeField] float grassEndHeight;

    [SerializeField] float dirtStartHeight;
    [SerializeField] float dirtEndHeight;

    [SerializeField] float stoneStartHeight;
    [SerializeField] float stoneEndHeight;

    [SerializeField] int chunkSize;

    [SerializeField] GameObject sword;
    [SerializeField] SeedGrowth basicSeed;
    [SerializeField] GameObject chestPrefab;
    [SerializeField] GameObject treePrefab;
    [SerializeField] Transform treesParent;
    [SerializeField] Item oreItem;
    private bool toUpdate = false;
    private bool swordCreated = false;
    public enum WorldClickType { Break,Till, Mine,Chop};
    Noise noise;
    Noise oreNoise;

    void Awake()
    {
        instance = this;
        if(randomSeed == true)
        {
            int value = Random.Range(-10000, 10000);
            seed = value;
        }
        noise = new Noise(seed,frequency,amplitude,lacunarity,persistence,octaves);
        if (randomSeed == true)
        {
            int value = Random.Range(-10000, 10000);
            oreSeed = value;
        }
        oreNoise = new Noise(oreSeed, frequency, amplitude, lacunarity, persistence, octaves);
        meshDict = new Dictionary<Vector2Int, Mesh>();
        tileMaps = new Dictionary<Vector2Int, Tile[,]>();
        
    }
    int getChunkSize()
    {
        return chunkSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        //CreateTiles();
        //SubdivideTilesArray();
        GenerateNewChunks(0, 0);
    }
    public int getWidth()
    {
        return width;
    }
    // Update is called once per frame
    void Update()
    {
        CheckForClick();
    }
    void CheckForClick()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Check if a UI button was clicked on
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(clickPosition.x, clickPosition.y), Vector2.zero, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (InventoryManager.instance.PlantableSelected() && World.instance.GetTileAt(Mathf.FloorToInt(clickPosition.x), Mathf.FloorToInt(clickPosition.y)).type == Tile.Type.Till)
            {
                InventoryManager.instance.ReduceSelectedItem();
                SeedGrowth seed = Instantiate(basicSeed, new Vector2(Mathf.FloorToInt(clickPosition.x) + 0.5f, Mathf.FloorToInt(clickPosition.y) + 0.5f), Quaternion.identity);
                seed.setPlanted();
            }
            if (!hit && PlayerMovement.instance.InPlayerReach(clickPosition))
            {
                if (Equipment.instance.handheld != null)
                {
                    WorldChangeClick(Mathf.FloorToInt(clickPosition.x), Mathf.FloorToInt(clickPosition.y), Equipment.instance.handheld.getClickType());
                }
                WorldChangeClick(Mathf.FloorToInt(clickPosition.x), Mathf.FloorToInt(clickPosition.y), WorldClickType.Break);
            }
            else if(Equipment.instance.handheld != null && Equipment.instance.handheld.getClickType() == WorldClickType.Chop && hit && PlayerMovement.instance.InPlayerReach(clickPosition) && hit.transform.gameObject.CompareTag("Tree"))
            {
                TreeBreak hitTree = hit.transform.gameObject.GetComponent<TreeBreak>();
                hitTree.SpawnWood();
            }
            

        }
    }
    public void WorldChangeClick(int x, int y,WorldClickType clickType)
    {
        Tile tile = GetTileAt(x, y);
        if (tile.getRequiredClickType() == clickType)
        {
            //tile.modifyTileType();
            SetTileAt(x, y, Tile.getModifiedStateFromType(tile.type));
        }
        Vector2Int offset = GetTilemapPos(x, y);
        SubdivideTilesArray(offset.x, offset.y, x, y);
    }
    public void ChunkGenerationCheck(int x, int y)
    {
        Vector2Int upOneChunk = GetTilemapPos(x, y + chunkSize);
        Vector2Int downOneChunk = GetTilemapPos(x, y - chunkSize);
        Vector2Int leftOneChunk = GetTilemapPos(x - chunkSize, y);
        Vector2Int rightOneChunk = GetTilemapPos(x + chunkSize, y);
        Vector2Int offset = GetTilemapPos(x,y);
        if (!tileMaps.ContainsKey(downOneChunk))
        {
            GenerateNewChunks(offset.x, offset.y - height);
        }
        if (!tileMaps.ContainsKey(upOneChunk))
        {
            GenerateNewChunks(offset.x, offset.y + height);
        }
        if (!tileMaps.ContainsKey(leftOneChunk))
        {
            GenerateNewChunks(offset.x - width, offset.y);
        }
        if (!tileMaps.ContainsKey(rightOneChunk))
        {
            GenerateNewChunks(offset.x + width, offset.y);
        }
    }
    public int getHeight()
    {
        return height;
    }
    void CreateTiles(int offsetX = 0,int offsetY = 0)
    {

        tiles = new Tile[width, height];
        Vector2Int tilemapPos = new Vector2Int(offsetX, offsetY);
        if (!tileMaps.ContainsKey(tilemapPos))
        {
            tileMaps.Add(tilemapPos, tiles);
        }
        float[,] noiseValues = noise.GetNoiseValues(width, height,offsetX,offsetY);
        float[,] oreNoiseValues = oreNoise.GetNoiseValues(width, height,offsetX,offsetY);
        for(int i = 0; i < width;i++)
        {
            for(int j =0;j<height;j++)
            {

                tiles[i, j] = MakeTileAtHeight(noiseValues[i, j]);
                if (tiles[i, j].type == Tile.Type.Stone && oreNoiseValues[i, j] < 0.3f)
                {
                    tiles[i, j] = new Tile(Tile.Type.OreStone);
                }
                if (tiles[i,j].type == Tile.Type.Dirt && oreNoiseValues[i,j] >= 0.8f && !swordCreated)
                {
                    GameObject aSword = Instantiate(sword, new Vector3(i+0.5f, j+0.5f, 0), Quaternion.identity);
                    aSword.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    swordCreated = true;
                }
                if (tiles[i, j].type == Tile.Type.Grass && oreNoiseValues[i, j] > 0.7f)
                {
                    tiles[i, j] = new Tile(Tile.Type.SeedGrass);
                }
                else if (tiles[i,j].type == Tile.Type.Grass && oreNoiseValues[i,j] <= 0.2f && Random.Range(0,3)>=2f)
                {
                    GameObject tree = Instantiate(treePrefab, new Vector3(i + offsetX + 0.5f, j + offsetY + 0.5f, 0), Quaternion.identity);
                    tree.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    tree.transform.SetParent(treesParent);
                }
                else if(tiles[i,j].type == Tile.Type.Grass && Mathf.FloorToInt(oreNoiseValues[i,j] * 1100) % 800 == 7)
                {
                    GameObject newChest = Instantiate(chestPrefab, new Vector3(i + offsetX + 0.5f, j + offsetY + 0.5f, 0), Quaternion.identity);
                    newChest.GetComponent<SpriteRenderer>().sortingLayerName = "Player"; 
                }
                
            }
        }
    }
    public Vector2Int GetTilemapPos(int x, int y,int w=0,int h=0)
    {
        if(w == 0 || h == 0)
        {
            w = width;
            h = height;
        }
        return new Vector2Int(x < 0 ? Mathf.FloorToInt((float)x / (float)w) * w : (x/w) * w, y < 0 ? Mathf.FloorToInt((float) y / (float)h) * h : (y/h)*h);
    }
    Tile MakeTileAtHeight(float currentHeight)
    {
        if(currentHeight <= seaLevel)
        {
            return new Tile(Tile.Type.Water);
        }
        if(currentHeight >= shallowWaterHeight && currentHeight <= shallowWaterEndHeight)
        {
            return new Tile(Tile.Type.ShallowWater);
        }
        if(currentHeight >= beachStartHeight && currentHeight <= beachEndHeight)
        {
            return new Tile(Tile.Type.Sand);
        }
        if (currentHeight >= dirtStartHeight && currentHeight <= dirtEndHeight)
            return new Tile(Tile.Type.Dirt);
        if (currentHeight >= grassStartHeight && currentHeight <= grassEndHeight)
            return new Tile(Tile.Type.Grass);

        if (currentHeight >= stoneStartHeight && currentHeight <= stoneEndHeight)
            return new Tile(Tile.Type.Stone);
        return new Tile(Tile.Type.Void);
    }
    public void GenerateNewChunks(int x, int y)
    {
        Vector2Int pos = new Vector2Int(x / width * width, y / height * height);
        if (!tileMaps.ContainsKey(pos))
        {
            CreateTiles(pos.x, pos.y);
            SubdivideTilesArray(pos.x, pos.y);
        }

    }
    void SubdivideTilesArray(int i1 = 0,int i2 = 0,int redrawX=0,int redrawY = 0)
    {

        Vector2Int offset = GetTilemapPos(i1, i2);//new Vector2Int(Mathf.FloorToInt((float)i1 / (float)width) * width, i2 / height * height);
        Tile[,] tiles = tileMaps[offset];
        /*
        if (redrawX != 0 && redrawY != 0)
        {
            Debug.Log("i1,i2 is " + i1 + "," + i2);
            Debug.Log("offset is" + offset.x + "," + offset.y);
            Debug.Log("redrawX,y is" + redrawX + "," + redrawY);
            Debug.Log("offsetX should be " + (i1 < 0 ? Mathf.FloorToInt((float)i1 / (float)width) * width : (i1 / width) * width));
        }*/
        if (i1 > tiles.GetLength(0) + offset.x && i2 > tiles.GetLength(1) + offset.y)
        {
            return;
        }
        if (redrawX != 0 && redrawY != 0 && redrawX < i1 && redrawY < i2)
        {
            return;
        }
        int sizeX;
        int sizeY;
        //Max position x = tiles.GetLength(0) + offset.x
        //Distance to max position x = diff(maxPosX,i1)
        //If adding chunkSize to i1 makes it go over the threshold X
        if (i1 + chunkSize > tiles.GetLength(0) + offset.x)//(tiles.GetLength(0) - i1 > chunkSize + offset.x)
        {
            sizeX = Mathf.Abs(tiles.GetLength(0) + offset.x - i1);
           
        }
        else
            sizeX = chunkSize;
        if (i2 + chunkSize > tiles.GetLength(1) + offset.y)//(tiles.GetLength(1) -i2 > chunkSize + offset.y)
        {
            sizeY = Mathf.Abs(tiles.GetLength(0) + offset.y - i2);
        }
        else
            sizeY = chunkSize;
        bool redrawInChunk = ((redrawX != 0 || redrawY != 0) && redrawX >= i1 && redrawX <= i1 + sizeX && redrawY >= i2 && redrawY <= i2 + sizeY);
        //If this isn't a redrawing, or redrawX and redrawY are within the chunk,
        //Then and only then bother drawing this chunk
        if ((redrawX == 0 && redrawY == 0) || redrawInChunk)
        {
            GenerateMesh(i1, i2, sizeX, sizeY,offset.x,offset.y);
        }
        if(redrawInChunk)
        {
            return;
        }
        

        //Do we have space on the x axis
        if(tiles.GetLength(0) >= i1 + chunkSize -offset.x)
        {
            SubdivideTilesArray(i1 + chunkSize, i2);
            return;
        }

        if(tiles.GetLength(1) >= i2 + chunkSize - offset.y)
        {
            SubdivideTilesArray(offset.x, i2 + chunkSize);
            return;
        }
    }
    void GenerateMesh(int x, int y,int width, int height,int offsetX = 0, int offsetY = 0)
    {
        Vector2Int loc = new Vector2Int(x,y);
        Mesh mesh;

        MeshData data = new MeshData(x,y,width,height,offsetX,offsetY);
        if (meshDict.ContainsKey(loc))
        {
            mesh = meshDict[loc];
            /*mesh.vertices = data.vertices.ToArray();
            mesh.triangles = data.triangles.ToArray();*/
            mesh.uv = data.UVs.ToArray();
            return;
            /*Destroy(meshDict[loc]);
            meshDict.Remove(loc);*/
        }
        GameObject meshGO = new GameObject("CHUNK_"+x+"_"+y);
        meshGO.transform.SetParent(this.transform);

        MeshFilter filter = meshGO.AddComponent<MeshFilter>();
        MeshRenderer render = meshGO.AddComponent<MeshRenderer>();
        render.material = material;

        mesh = filter.mesh;

        mesh.vertices = data.vertices.ToArray();
        mesh.triangles = data.triangles.ToArray();
        mesh.uv = data.UVs.ToArray();
        meshDict.Add(new Vector2Int(x, y), mesh);
    }
    public void SetTileAt(int x,int y,Tile.Type type)
    {
        Vector2Int offset = GetTilemapPos(x, y);
        tiles = tileMaps[offset];
        int adjustedX = x - offset.x;
        int adjustedY = y - offset.y;
        if(tiles[adjustedX,adjustedY].type.ToString().Contains("Seed"))
        {
            OnSeedBreak(x,y,basicSeed);
        }
        else if(tiles[adjustedX,adjustedY].type.ToString().Contains("Ore"))
        {
            OnOreBreak(x, y,oreItem);
        }
        /*
        tiles[adjustedX,adjustedY].type = type;
        
        tiles[adjustedX, adjustedY].tileClass = Tile.getClassFromType(type);*/
        tiles[adjustedX,adjustedY].modifyTileType(type);
    }
    public void OnOreBreak(int x, int y, Item ore)
    {
        InventoryManager.instance.AddItem(ore);
    }
    public void OnSeedBreak(int x,int y,SeedGrowth prefab)
    {
        if (Random.Range(-1,5) == 2)
        {
            Instantiate(prefab, new Vector3(x+0.5f, y+0.5f), Quaternion.identity);
        }
    }
    public Tile GetTileAt(int x,int y)
    {
        Vector2Int offset1 = GetTilemapPos(x, y);
        if (tileMaps.ContainsKey(offset1))
        {

            tiles = tileMaps[offset1];
            return tiles[x - offset1.x, y - offset1.y];
        }
        else
        {
            return null;
        }
    }
}
