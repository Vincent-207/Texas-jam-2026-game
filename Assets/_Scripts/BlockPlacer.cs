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
    private InputActionReference clickAction, mousePos, rotateInput;
    GridController gridController;
    private Camera cam;

    [SerializeField]
    private SpriteRenderer previewSpriteIcon;

    BlockSelectable selectable;
    [SerializeField] public Vector2Int blockPlaceDir = Vector2Int.right;
    private bool canPlace = true;
    public void DisablePlacing()
    {
        canPlace = false;
        currentBlockPrefab = null;
        availableAmount = 0;
        previewSpriteIcon.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        UpdatePreview();
        
        BlockSelectable[] blockSelectables = FindObjectsByType<BlockSelectable>();
        foreach (BlockSelectable blockSelectable in blockSelectables)
        {
            blockSelectable.Disable();
        }
    }
    void Awake()
    {
        gridController = FindAnyObjectByType<GridController>();
        // inputActions.UI.Click.started += OnClick;
        clickAction.action.started += OnClick;
        rotateInput.action.started += RotateBlock;
        cam = Camera.main;
    }

    void Start()
    {
        previewSpriteIcon.transform.localScale = gridController.transform.localScale;
        
        UpdatePreview();
    }

    void RotateBlock(InputAction.CallbackContext context)
    {
        if (previewSpriteIcon == null) return;
        float angle = previewSpriteIcon.transform.eulerAngles.z;
        previewSpriteIcon.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        blockPlaceDir = new Vector2Int(blockPlaceDir.y, -blockPlaceDir.x);
    }

    private void OnDisable()
    {
        clickAction.action.started -= OnClick;
    }

    public void SetBlock(GameObject blockPrefab, int count, BlockSelectable selectable)
    {
        if(!canPlace) return;
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
        GameObject block = Instantiate(currentBlockPrefab);
        bool successfullyPlaced = gridController.TryAddBlock(gridController.WorldToGridPos(mouseWorldPos), block.GetComponent<Block>());
        if (successfullyPlaced)
        {
            availableAmount--;
            UpdatePreview();
            UpdateSelectable();
            IRotateable rotateable = block.GetComponent<IRotateable>();
            if(rotateable != null) rotateable.SetRotate(blockPlaceDir);
        }
        else
        {
            Destroy(block);
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
