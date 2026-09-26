using System.IO;
using UnityEngine;

public class PullBlock : Block
{
    public Vector2Int direction = Vector2Int.right;
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        this.pos = pos;
    }
    
    public override void Process()
    {
        // Debug.Log("Processing Kinetic Block!");
       
        Vector2Int newCoords = pos + direction;

        if (!AreCordsInGrid(newCoords)) return;
        
        
        Block nextTile = blocks[newCoords.x, newCoords.y];
        if(nextTile == null || (nextTile != null && nextTile.CanPush(direction)))
        {
            if(nextTile != null) nextTile.Push(direction);
            TryToPull();
            pos += direction;
            // Debug.Log("Updated pos: " + pos);
        }
    }

    void TryToPull()
    {
        //Get closest block left.
        Block closestBlockBehind = null;
        Vector2Int coordsBehind = pos - direction;
        while (closestBlockBehind == null)
        {
            Debug.Log("pos: "  + pos);
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
            return;
        }
        else if (closestBlockBehind.CanPush(direction))
        {
            Debug.Log("Pulling closest block!");
            closestBlockBehind.Push(direction);
        }
        else
        {
            Debug.Log("Can't pull it");
            return;
        }
    }
}
