using System.IO;
using UnityEngine;

public class PullBlock : Block
{
    public Vector2Int direction = Vector2Int.right;
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }
    
    public override float Process()
    {
        // Debug.Log("Processing Kinetic Block!");
       
        Vector2Int newCoords = currentPos + direction;

        if (!AreCordsInGrid(newCoords)) return 0f;
        
        
        Block nextTile = blocks[newCoords.x, newCoords.y];
        if(nextTile == null || (nextTile != null && nextTile.CanPush(direction)))
        {
            if(nextTile != null) nextTile.Push(direction);
            float pullTime = TryToPull();
            nextPos += direction;
            float animTime = TileAnimator.DoPushTween(direction, transform.parent) + pullTime;
            return animTime;
            // Debug.Log("Updated pos: " + pos);
        }

        return 0f;
    }

    float TryToPull()
    {
        //Get closest block left.
        Block closestBlockBehind = null;
        Vector2Int coordsBehind = currentPos - direction;
        while (closestBlockBehind == null)
        {
            Debug.Log("pos: "  + currentPos);
            Debug.Log("coordsBehind: " + coordsBehind);
            if (!AreCordsInGrid(coordsBehind)) break;
            
            if (blocks[coordsBehind.x, coordsBehind.y] == null)
            {
                coordsBehind -= direction;
                Debug.Log("new coordsBehind: " + coordsBehind);
            }
            else
            {
                closestBlockBehind = blocks[coordsBehind.x, coordsBehind.y];
                Debug.Log("found block behind: " + closestBlockBehind.gameObject.name);
            }
        }

        if (closestBlockBehind == null)
        {
            Debug.Log("Nothing to pull!");
            return 0f;
        }
        else if (closestBlockBehind.CanPush(direction))
        {
            Debug.Log("Pulling closest block!");
            closestBlockBehind.Push(direction);
            return TileAnimator.DoPushTween(direction, closestBlockBehind.transform.parent);
        }
        else
        {
            Debug.Log("Can't pull it");
            return 0f;
        }
    }
}
