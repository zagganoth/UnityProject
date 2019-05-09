using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    public BoxCollider2D collide;
    public static PlayerMovement instance;
    public enum Direction { Up,Down,Left,Right };
    public Direction direction;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collide = GetComponent<BoxCollider2D>();
        direction = Direction.Down;
    }

    // Update is called once per frame
    void Update()
    {
        MenuOpenCheck();
        RegisterMovement();

    }
    void MenuOpenCheck()
    {
        if (Input.GetButtonDown("Inventory"))
        {
  
            Equipment.instance.playerInventory.gameObject.SetActive(!Equipment.instance.playerInventory.gameObject.activeSelf);
        }
    }
    void RegisterMovement()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        UpdateAnimationAndMove();
    }
    public void SetDirection(float x, float y)
    {
        if (!animator.GetBool("moving"))
        {
            if (x > 1 || x < -1)
            {
                animator.SetFloat("moveX", x);
                animator.SetFloat("moveY", 0);
            }
            if (y > 1 || y < -1)
            {
                animator.SetFloat("moveY", y);
                animator.SetFloat("moveX", 0);
            }
        }
        direction = (x >= 1) ? Direction.Right : (x <= -1 ? Direction.Left : direction);

        
        direction = (y >= 1) ? Direction.Up : (y <= -1 ? Direction.Down : direction);
    }
    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {      
            animator.SetBool("moving", true);

            animator.SetFloat("moveX", change.x);
            direction = (change.x > 0) ? Direction.Right : Direction.Left;

            animator.SetFloat("moveY", change.y);
            direction = (change.y > 0) ? Direction.Up : Direction.Down;
            MoveCharacter();
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
    void MoveCharacter()
    {
        if (World.instance != null)
        {
            CollisionCheck();
        }
       myRigidBody.MovePosition(transform.position + change * speed * Time.deltaTime);
        
    }
    public bool InPlayerReach(Vector2 clickPos)
    {
        return Vector2.Distance(clickPos, transform.position) <= Equipment.instance.getReach();
    }
    private void CollisionCheck()
    {
        int nextTileX = Mathf.FloorToInt(collide.transform.position.x + change.x);
        int nextTileY = Mathf.FloorToInt(collide.transform.position.y + collide.offset.y + change.y);

        Tile curTileY = World.instance.GetTileAt(Mathf.FloorToInt(collide.transform.position.x), nextTileY);
        Tile curTileX = World.instance.GetTileAt(nextTileX, Mathf.FloorToInt(collide.transform.position.y + collide.offset.y));
        if (change.x < 0) nextTileX += 1;
        if (change.y < 0) nextTileY += 1;
        float distX = 0f;
        float distY = 0f;
        if (curTileX != null && curTileX.tileClass == Tile.Class.Collidable)
        {
            distX = (float)nextTileX - collide.transform.position.x;
            if (distX > 0.1f || distX < -0.1f)
                change.x = distX;
            else
                change.x = 0f;
        }
        if (curTileY != null && curTileY.tileClass == Tile.Class.Collidable)
        {
            distY = (float)nextTileY - (collide.transform.position.y + collide.offset.y);

            if (distY > 0.1f || distY < -0.1f)
                change.y = distY;
            else
                change.y = 0f;
        }
        World.instance.ChunkGenerationCheck(nextTileX, nextTileY);
        /*
        Vector2Int offset = World.instance.GetTilemapPos(nextTileX, nextTileY);
        if(curTileX == null)
        {
            World.instance.GenerateNewChunks(nextTileX - World.instance.getWidth(), offset.y);
            return;
        }
        if(curTileY == null)
        {
            World.instance.GenerateNewChunks(offset.x, nextTileY - World.instance.getHeight());
            return;
        }*/
    }
    void OnCollisionEnter2D(Collision2D col)
    {
    }
}
