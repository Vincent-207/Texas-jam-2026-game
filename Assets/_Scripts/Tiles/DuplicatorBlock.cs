using UnityEngine;

public class DuplicatorBlock : Block, IRotateable
{
    public Vector2Int direction = Vector2Int.right;
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        nextPos = currentPos = pos;
    }

    public override float Process()
    {
        Vector2Int behind = new Vector2Int(currentPos.x - direction.x, currentPos.y - direction.y);
        Vector2Int target = new Vector2Int(currentPos.x + direction.x, currentPos.y + direction.y);
        if (!AreCordsInGrid(behind) || !AreCordsInGrid(target)) return 0f;

        Block dupeTile = blocks[behind.x, behind.y];
        Block targetTile = blocks[target.x, target.y];

        if (dupeTile != null && targetTile == null)
        {
            Block newBlock = Instantiate(dupeTile);
            Debug.Log("newBlock: " + newBlock);
            newBlock.SetValues(blocks, target);
            blocks[target.x, target.y] = newBlock;
            Debug.Log("newBlock pos: " + newBlock.currentPos);
        }

        return 0f;
    }

    public override bool CanPush(Vector2Int direction)
    {
        if (direction == this.direction)
        {
            Vector2Int behind = new Vector2Int(currentPos.x - direction.x, currentPos.y - direction.y);
            Vector2Int target = new Vector2Int(currentPos.x + direction.x, currentPos.y + direction.y);
            Block dupeTile = blocks[behind.x, behind.y];
            Block targetTile = blocks[target.x, target.y];

            if (dupeTile != null && targetTile == null) return false;
            else if (targetTile != null) return false;
        }
        else
        {
            Vector2Int newPos = currentPos + direction;
            if (!AreCordsInGrid(newPos)) return false;
            if (blocks[newPos.x, newPos.y] != null) return false;
        }
        return true;
    }

    public void SetRotate(Vector2Int dir)
    {
        this.direction = dir;
    }

    public bool isClockWiseRotateable()
    {
        return false;
    }

    public Vector2Int GetDirection()
    {
        return direction;
    }
}
