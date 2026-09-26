using UnityEngine;

public class WallBlock : Block
{
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }

    public override float Process()
    {
        return 0f;
    }

    public override bool CanPush(Vector2Int direction)
    {
        return false;
    }

    public override void Push(Vector2Int direction)
    {
        base.Push(direction);
        Debug.Log("Getting pushed to: " + nextPos);
    }
}
