using System;
using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField]
    private Block block;

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
        if (block == null) return;
        block.transform.position = transform.position;
        block.gameObject.transform.parent = transform;
        sprite.color = block.TileColor;
    }
    
}
