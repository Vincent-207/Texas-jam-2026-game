using UnityEngine;

public class ObjectiveBlock : Block
{
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        this.pos = pos;
    }

    public override void Process()
    {
        return;
    }
}
