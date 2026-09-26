using UnityEngine;

public class ReactionBlock : Block
{
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }

    public override void Process()
    {
        return;
    }

    public override void Push(Vector2Int direction)
    {
        base.Push(direction);
        
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Block targetBlock = blocks[currentPos.x + i, currentPos.y + j];
                if (targetBlock != null && targetBlock != this)
                {
                    if (targetBlock.CanPush(direction))
                    {
                        targetBlock.Push(direction);
                    }
                }
            }
        }
    }
}
