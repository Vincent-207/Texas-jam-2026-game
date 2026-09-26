using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject currentBlockPrefab = null;
    [SerializeField]
    private int availableAmount = 0;
    [SerializeField]
    private InputActionReference clickAction, mousePos;
    GridController gridController;
    private Camera cam;

    [SerializeField]
    private SpriteRenderer previewSpriteIcon;

    BlockSelectable selectable;
    void Awake()
    {
        gridController = FindAnyObjectByType<GridController>();
        // inputActions.UI.Click.started += OnClick;
        clickAction.action.started += OnClick;
        
        cam = Camera.main;
    }

    void Start()
    {
        previewSpriteIcon.transform.localScale = gridController.transform.localScale;
        
        UpdatePreview();
    }

    private void OnDisable()
    {
        clickAction.action.started -= OnClick;
    }

    public void SetBlock(GameObject blockPrefab, int count, BlockSelectable selectable)
    {
        currentBlockPrefab = blockPrefab;
        availableAmount = count;
        UpdatePreview();
        this.selectable = selectable;
    }

    void UpdatePreview()
    {
        if(availableAmount <= 0 || currentBlockPrefab == null) previewSpriteIcon.color = new Color32(255, 255, 255, 0);
        else
        {
            previewSpriteIcon.enabled = true;
            previewSpriteIcon.color = currentBlockPrefab.GetComponent<Block>().TileColor;
        }
        
        
    }

    void Update()
    {
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mousePos.action.ReadValue<Vector2>());
        if (gridController.IsPointOverGrid(mouseWorldPos))
        {
            previewSpriteIcon.enabled = true;
            // previewSpriteIcon.transform.position = (Vector2) gridController.WorldToGridPos(mouseWorldPos);
        }
        else
        {
            previewSpriteIcon.enabled = false;
        }

        Vector2 scaledMousePos = new Vector2(mouseWorldPos.x / gridController.transform.localScale.x,
            mouseWorldPos.y / gridController.transform.localScale.y);
        Vector2Int roundedPos = Vector2Int.RoundToInt(scaledMousePos);
        Vector2 deScaledPos = new Vector2(roundedPos.x * gridController.transform.localScale.x, roundedPos.y * gridController.transform.localScale.y);
        
        previewSpriteIcon.transform.position = deScaledPos;
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("Clicked!");
        
        if (availableAmount <= 0) return;
        
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mousePos.action.ReadValue<Vector2>());
        bool successfullyPlaced = gridController.TryAddBlock(gridController.WorldToGridPos(mouseWorldPos), currentBlockPrefab);
        if (successfullyPlaced)
        {
            availableAmount--;
            UpdatePreview();
            UpdateSelectable();
        }
    }

    // void PlaceBlock(Vector2 mouseWorldPos)
    // {
    //     Block createdBlock = Instantiate(currentBlock);
    //     Debug.Log("Placing block!");
    //     GridTile tile = gridController.GetTile(mouseWorldPos);
    //     Debug.Log("Found tile");
    //     gridController.GetTile(mouseWorldPos).SetBlock(createdBlock);
    //     Debug.Log("Now updating");
    //     availableAmount--;
    //     UpdatePreview();
    //     UpdateSelectable();
    //     Debug.Log("Done placing block!");
    // }

    void UpdateSelectable()
    {
        selectable.SetCount(availableAmount);
        
    }
    
    
    
    
    
}
