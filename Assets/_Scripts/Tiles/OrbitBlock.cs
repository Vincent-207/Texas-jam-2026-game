using UnityEngine;

public class OrbitBlock : Block, IRotateable
{
    public int direction = 1; // 1 is counterclockwise, -1 is clockwise
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }

    public override float Process()
    {
        Debug.Log("Processing Orbit Block!");
        int validNeighbors = 0;
        float time = 0f;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!AreCordsInGrid(new Vector2Int(currentPos.x + i, currentPos.y + j))) continue;

                Block tile = blocks[currentPos.x + i, currentPos.y + j];
                if (tile != null && tile != this)
                {
                    int x = (currentPos.x + i - currentPos.x);
                    int y = (currentPos.y + j - currentPos.y);

                    Vector2Int rotation;
                    if (x == y * direction) rotation = new Vector2Int(-y, 0);
                    else if (x == -y * direction) rotation = new Vector2Int(0, x);
                    else rotation = new Vector2Int(-y, x);
                    rotation *= new Vector2Int(direction, direction);

                    if (tile.CanPush(rotation))
                    {
                        tile.Push(rotation);
                        time = TileAnimator.DoPushTween(rotation, tile.transform.parent);
                    }
                    validNeighbors++;
                    Debug.Log("Pushed " + tile + ": " + rotation);
                    
                }
            }
        }

        return time;
    }

    public void SetRotate(Vector2Int dir)
    {
        if (dir.y == 0) this.direction = 1;
        else this.direction = -1;

        Debug.Log("Actually Happened: " + direction);
    }
}