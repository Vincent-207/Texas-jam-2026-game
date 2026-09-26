using System.Collections.Generic;
using UnityEngine;
//Anims: working
//Overlays: working
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
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector2Int pos = new Vector2Int(x + currentPos.x, y + currentPos.y);
                if (!AreCordsInGrid(pos)) continue;

                Block block = neighbors[(y + 1) * 3 + (x + 1)] = blocks[pos.x, pos.y];
                if(x == 0 && y == 0) continue;
                if (block != null)
                {
                    Debug.Log(block.name);
                    Debug.Log(x + ", " + y);
                    Vector2Int pushDir = new Vector2Int(x, y);
                    if(block.CanPush(pushDir)) block.Push(pushDir);
                    pushDirections.Add(pushDir);
                    overlayManager.AddOverlay(pushDir, block.GetComponentInParent<GridTile>());
                }
            }
        }
        
        float dur = TileAnimator.DoPushMultipleTween(transform.parent, pushDirections.ToArray());

        return dur;
    }
}
