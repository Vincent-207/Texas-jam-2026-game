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
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                
                Block block = neighbors[y * 3 + x] = blocks[x + currentPos.x - 1, y + currentPos.y - 1];

                if (block != null)
                {
                    Debug.Log(block.name);
                    Debug.Log(x + ", " + y);
                    Vector2Int pushDir = new Vector2Int(x - 1, y - 1);
                    if(block.CanPush(pushDir)) block.Push(pushDir);
                }
            }
        }

        return 0f;
    }
}
