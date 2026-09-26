using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class KineticBlock : Block
{
    public Vector2Int direction = Vector2Int.right;
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }

    public override void Process()
    {
        
        Vector2Int newPos = nextPos + direction;
        if (!AreCordsInGrid(newPos))
            return;
        
        Block nextTile = blocks[currentPos.x + direction.x, currentPos.y + direction.y];
        if(nextTile == null || (nextTile != null && nextTile.CanPush(direction)))
        {
            if(nextTile != null) nextTile.Push(direction);
            nextPos += direction;
            // Debug.Log("Updated pos: " + pos);
        }
    }

  

}
