using UnityEngine;

public abstract class Block : MonoBehaviour
{
    
    public Block[,] blocks;
    public Vector2Int pos;
    public abstract void SetValues(Block[,] tiles, Vector2Int pos);
    public abstract void Process();
    [SerializeField]
    private Color tileColor;
    public Color TileColor => tileColor;
    

    public virtual bool CanPush(Vector2Int direction)
    {
        
        Vector2Int newPos = pos + direction;
        
        if (blocks[newPos.x, newPos.y] != null) return false;
        
        //check if it will be pushed out of bounds to prevent error
        if (newPos.y < 0 || newPos.y >= blocks.Length)
        {
            return false;
        }
        else if (newPos.x < 0 || newPos.x >= blocks.GetLength(0))
        {
            return false;
        }
        else return true;
    }

    public virtual void Push(Vector2Int direction)
    {
        pos += direction;
    }
}
