using System;
using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField]
    private Block block;

    [SerializeField] private Sprite defaultSprite;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public Block GetBlock()
    {
        return block;
    }

    public void SetBlock(Block newBlock)
    {
        block = newBlock;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (block == null)
        {
            sprite.color = Color.black;
            sprite.sprite = defaultSprite;
            // transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        block.transform.position = transform.position;
        block.gameObject.transform.parent = transform;
        sprite.color = block.TileColor;
        sprite.sprite = block.sprite;
        IRotateable rotateable = block.GetComponent<IRotateable>();
        if (rotateable != null)
        {
            Vector2Int direction = rotateable.GetDirection();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
            
        }
    }
    
    public void DebugSetColor(Color color)
    {
        sprite.color = color;
    }
}
