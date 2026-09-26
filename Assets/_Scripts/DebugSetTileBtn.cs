using System;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class DebugSetTileBtn : MonoBehaviour
{
    [SerializeField] private GridController grid; 
    [SerializeField] GameObject blockPrefab;
    private Button btn;
    public Vector2Int pos;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SetTile);
    }

    void SetTile()
    {
        Block block = Instantiate(blockPrefab).GetComponent<Block>();
        grid.getTile(pos).SetBlock(block);
        
    }
}
