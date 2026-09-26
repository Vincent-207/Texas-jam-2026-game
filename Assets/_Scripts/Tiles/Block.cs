using UnityEngine;

public abstract class Block : MonoBehaviour
{
    
    public Block[,] blocks;
    public Vector2Int pos;
    public abstract void SetValues(Block[,] newBlocks, Vector2Int pos);
    public abstract void Process();
    [SerializeField]
    private Color tileColor;
    public Color TileColor => tileColor;
    

    public virtual bool CanPush(Vector2Int direction)
    {
        
        Vector2Int newPos = pos + direction;
        if(!AreCordsInGrid(newPos)) return false;
        if (blocks[newPos.x, newPos.y] != null) return false;
        return true;
    }

    public virtual bool CanPush(Vector2Int direction, int width)
    {
        //width is how far this will keep checking right before returning false.
        Vector2Int newPos = pos + direction;
        if(!AreCordsInGrid(newPos)) return false;
        if (blocks[newPos.x, newPos.y] != null) return false;
        return true;
        
    }

    public virtual void Push(Vector2Int direction)
    {
        Debug.Log("I : " + gameObject.name);
        pos += direction;
        Debug.Log("Getting pushed to: " + pos);
    }

    public virtual bool AreCordsInGrid(Vector2Int cords)
    {
        if (cords.y < 0 || cords.y >= blocks.Length)
        {
            return false;
        }
        else if (cords.x < 0 || cords.x >= blocks.GetLength(0))
        {
            return false;
        }
        else return true;
    }
    
    
}
