using System.Collections.Generic;
using UnityEngine;

public class PushAllBlock : Block
{
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }

    public override float Process()
    {
        Block[] neighbors = new Block[9];
        List<Vector2Int> pushDirections = new();
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                Vector2Int pos = new Vector2Int(x + currentPos.x - 1, y + currentPos.y - 1);
                if (!AreCordsInGrid(pos)) continue;

                Block block = neighbors[y * 3 + x] = blocks[pos.x, pos.y];
                if(x == 1 && y == 1) continue;
                if (block != null)
                {
                    Debug.Log(block.name);
                    Debug.Log(x + ", " + y);
                    Vector2Int pushDir = new Vector2Int(x - 1, y - 1);
                    if(block.CanPush(pushDir)) block.Push(pushDir);
                    pushDirections.Add(pushDir);
                }
            }
        }
        
        float dur = TileAnimator.DoPushMultipleTween(transform.parent, pushDirections.ToArray());

        return dur;
    }
}
