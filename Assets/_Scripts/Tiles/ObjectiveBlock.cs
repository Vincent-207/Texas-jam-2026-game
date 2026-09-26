using UnityEngine;

public class ObjectiveBlock : Block
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
        Debug.Log("Getting pushed to: " + nextPos);
    }
}
