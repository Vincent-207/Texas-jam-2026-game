using System.Reflection.Metadata.Ecma335;
using UnityEngine;
//ANIMS: working
//Overlays: working
public class KineticBlock : Block, IRotateable
{
    public Vector2Int direction = Vector2Int.right;
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }

    void Start()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation =  Quaternion.Euler(0f, 0f, angle);
    }

    public override float Process()
    {
        
        Vector2Int newPos = nextPos + direction;
        if (!AreCordsInGrid(newPos))
            return 0f;
        
        Block nextTile = blocks[currentPos.x + direction.x, currentPos.y + direction.y];
        if(nextTile == null || (nextTile != null && nextTile.CanPush(direction)))
        {
            //Successfully do push.
            
            if(nextTile != null) nextTile.Push(direction);
            nextPos += direction;
            float time = TileAnimator.DoPushGraphics(direction, transform.parent);
            overlayManager.AddOverlay(direction, transform.parent.GetComponent<GridTile>());
            // Debug.Log(nextTile.transform.parent.name);
            if(nextTile != null) overlayManager.AddOverlay(direction, nextTile.GetComponentInParent<GridTile>());
            // Debug.Log("Updated pos: " + pos);
            return time;
        }

        return 0f;
    }

    public override bool IsAnimating()
    {
        return base.IsAnimating();
    }

    public void SetRotate(Vector2Int dir)
    {
        this.direction = dir;
    }

    public bool isClockWiseRotateable()
    {
        return false;
    }

    public Vector2Int GetDirection()
    {
        return direction;
    }

    public int GetClockWiseDir()
    {
        return 0;
    }
}

public interface IRotateable
{
    public void SetRotate(Vector2Int direction);
    public bool isClockWiseRotateable();
    public Vector2Int GetDirection();
    public int GetClockWiseDir();
}
