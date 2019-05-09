using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handheld : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer render;
    public enum Class { Weapon, Hoe, Axe, Pickaxe};
    [SerializeField] int reach;
    public Class itemClass;
    private PlayerMovement.Direction direction;
    private ItemPickup pickup;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        direction = PlayerMovement.Direction.Down;
        pickup = GetComponent<ItemPickup>();
    }
    public void SetAnimationBool(string boolName,int swingPosX,int swingPosY)
    {
        StartCoroutine(AnimationRoutine(boolName,swingPosX,swingPosY));
        //SpriteRenderer
        //animator.SetBool(boolName, true);

    }
    public int getReach()
    {
        return reach;
    }
    public void WorldClicked(float x, float y)
    {
        pickup.enabled = false;
        float playerX = PlayerMovement.instance.collide.transform.position.x;
        float playerY = PlayerMovement.instance.collide.transform.position.y-0.5f;
        int swingPosX = (int)playerX;
        int swingPosY = (int)playerY;
        //if (itemClass == Class.Weapon)
        //{
        string animationBool = "SwingDown";
        int diffX = (int)(x - playerX);
        int diffY = (int)(y - playerY);
        PlayerMovement.instance.SetDirection(diffX, diffY);
        switch (PlayerMovement.instance.direction)
        {
            case PlayerMovement.Direction.Up:
                //SetAnimationBool("SwingUp");
                animationBool = "SwingUp";
                transform.position = new Vector2(playerX, playerY + 1);
                break;
            case PlayerMovement.Direction.Down:
                //SetAnimationBool("SwingDown");
                animationBool = "SwingDown";
                transform.position = new Vector2(playerX, playerY - 1);
                break;
            case PlayerMovement.Direction.Left:
                //SetAnimationBool("SwingLeft");
                animationBool = "SwingLeft";
                transform.position = new Vector2(playerX - 1, playerY);
                break;
            case PlayerMovement.Direction.Right:
                //SetAnimationBool("SwingRight");
                animationBool = "SwingRight";
                transform.position = new Vector2(playerX + 1, playerY);
                break;
        }
        swingPosX = (int)playerX + (Mathf.Abs(diffX) > reach  ? reach * (diffX >= 0 ? 1 : -1) : diffX);
        swingPosY = (int)playerY + (Mathf.Abs(diffY) > reach  ? reach * (diffY >= 0 ? 1 : -1) : diffY);
        SetAnimationBool(animationBool,swingPosX, swingPosY);

    }
    public World.WorldClickType getClickType()
    {
        if(itemClass == Class.Hoe)
        {
            return World.WorldClickType.Till;
        }
        if(itemClass == Class.Pickaxe)
        {
            return World.WorldClickType.Mine;
        }
        if(itemClass == Class.Axe)
        {
            return World.WorldClickType.Chop;
        }
        else
        {
            return World.WorldClickType.Break;
        }
    }
    private IEnumerator AnimationRoutine(string boolName,int swingPosX,int swingPosY)
    {
        animator.SetBool(boolName, true);

        if (itemClass == Class.Hoe)
        {
            World.instance.WorldChangeClick(swingPosX, swingPosY, World.WorldClickType.Till);
        }
        else if(itemClass == Class.Pickaxe)
        {
            World.instance.WorldChangeClick(swingPosX, swingPosY, World.WorldClickType.Mine);
        }
        else if(itemClass == Class.Axe)
        {
            if (PlayerMovement.instance.InPlayerReach(new Vector2(swingPosX, swingPosY)))
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (hit)
                {

                    if (hit.transform.gameObject.CompareTag("Tree"))
                    {
                        TreeBreak hitTree = hit.transform.gameObject.GetComponent<TreeBreak>();
                        //hitTree.SpawnWood(swingPosX, swingPosY);
                    }

                }
            }
        }
        yield return new WaitForSeconds(0.16f);
        gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
