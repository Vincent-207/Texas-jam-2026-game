using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class KineticBlock : Block
{
    public Vector2Int direction = Vector2Int.right;
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        this.pos = pos;
    }

    public override void Process()
    {
        Debug.Log("Processing Kinetic Block!");
        int x = pos.x + direction.x;
        int y = pos.x + direction.y;

        if ((x < 0 || x >= blocks.GetLength(0) || y < 0 || y >= blocks.GetLength(1)))
            return;
        
        Block nextTile = blocks[pos.x + direction.x, pos.y + direction.y];
        if(nextTile == null || (nextTile != null && nextTile.CanPush(direction)))
        {
            if(nextTile != null) nextTile.Push(direction);
            pos += direction;
            Debug.Log("Updated pos: " + pos);
        }
    }

  

}
