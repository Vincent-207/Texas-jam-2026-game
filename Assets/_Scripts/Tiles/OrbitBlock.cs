using UnityEngine;

public class OrbitBlock : Block
{
    public int direction = 1; // 1 is counterclockwise, -1 is clockwise
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        this.pos = pos;
    }

    public override void Process()
    {
        Debug.Log("Processing Orbit Block!");
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!AreCordsInGrid(new Vector2Int(pos.x + i, pos.y + j))) continue;

                Block tile = blocks[pos.x + i, pos.y + j];
                if (tile != null && tile != this)
                {
                    int x = (pos.x + i - pos.x);
                    int y = (pos.y + j - pos.y);

                    Vector2Int rotation;
                    if (x == y * direction) rotation = new Vector2Int(-y, 0);
                    else if (x == -y * direction) rotation = new Vector2Int(0, x);
                    else rotation = new Vector2Int(-y, x);
                    rotation *= new Vector2Int(direction, direction);

                    if (tile.CanPush(rotation)) tile.Push(rotation);
                    Debug.Log("Pushed " + tile + ": " + rotation);
                }
            }
        }
    }
}
