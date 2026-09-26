using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    [SerializeField] private TileInfo[] tiles;
    public TileInfo[] Tiles => tiles;
}

[System.Serializable]
public class TileInfo
{
    [SerializeField]
    private Vector2Int position;
    public Vector2Int Position => position;
    [SerializeField]
    private GameObject blockPrefab;
    public GameObject BlockPrefab => blockPrefab;

    public TileInfo(Vector2Int position, GameObject blockPrefab)
    {
        this.position = position;
        this.blockPrefab = blockPrefab;
    }
    
}
