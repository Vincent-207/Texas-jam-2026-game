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

    [SerializeField] private Sprite vectorDirSprite, clockwiseDirSprite;
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
        
        cam = Camera.main;
    }
    

    private void OnEnable()
    {
        clickAction.action.started += OnClick;
        rotateInput.action.started += RotateBlock;
    }

    private void OnDisable()
    {
        clickAction.action.started -= OnClick;
        rotateInput.action.started -= RotateBlock;
    }

    void Start()
    {
        previewSpriteIcon.transform.localScale = gridController.transform.localScale;
        UpdatePreview();
        SetBlock(null, 0, null);
    }

    void RotateBlock(InputAction.CallbackContext context)
    {
        Debug.Log("Rotating block!");
        if (previewSpriteIcon == null) return;
        if (currentBlockPrefab == null) return;
        if (IsBlockRotateable())
        {
            blockPlaceDir = new Vector2Int(blockPlaceDir.y, -blockPlaceDir.x);
            float angle = previewSpriteIcon.transform.eulerAngles.z;
            Debug.Log("Angle: " + angle);
            previewSpriteIcon.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        UpdatePreview();

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
        SpriteRenderer rotationDisplay = previewSpriteIcon.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(availableAmount <= 0 || currentBlockPrefab == null)
        {
            Debug.Log("No prefab or little amount!");
            previewSpriteIcon.enabled = false;
            rotationDisplay.enabled = false;
        }
        else
        {
            Block previewBlock = currentBlockPrefab.GetComponent<Block>();
            previewSpriteIcon.enabled = true;
            previewSpriteIcon.color = previewBlock.TileColor;
            previewSpriteIcon.enabled = true;
            rotationDisplay.enabled = true;
            previewSpriteIcon.sprite = previewBlock.sprite;
        }
        
        Debug.Log("Hit this part!");
        rotationDisplay.sprite = null;
        if (IsBlockRotateable())
        {
            Debug.Log("Is rotateable");
            Vector3 normalScale = rotationDisplay.transform.localScale;
            normalScale.x = Mathf.Abs(normalScale.x);
            rotationDisplay.transform.localScale = normalScale;
            if (IsBlockClockWiseTypeRotate())
            {
                rotationDisplay.sprite = clockwiseDirSprite;
                Vector3 flippedScale = rotationDisplay.transform.localScale;
                if (blockPlaceDir == Vector2Int.right || blockPlaceDir == Vector2Int.left)
                {
                    // rotationDisplay.sprite = clockwiseDirSprite;
                    flippedScale.x = -Mathf.Abs(flippedScale.x);
                    rotationDisplay.transform.localScale = flippedScale;
                }
                else
                {
                        
                    flippedScale.x = Mathf.Abs(flippedScale.x);
                    rotationDisplay.transform.localScale = flippedScale;
                }   
            }
            else
            {
                rotationDisplay.sprite = vectorDirSprite;
            }
        }
        
        
    }

    void Update()
    {
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mousePos.action.ReadValue<Vector2>());
        bool a = gridController.IsPointOverGrid(mouseWorldPos);
        // Debug.Log("Over grid: " +  a);
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
        IRotateable rotateable = block.GetComponent<IRotateable>();
        if(rotateable != null) rotateable.SetRotate(blockPlaceDir);
        bool successfullyPlaced = gridController.TryAddBlock(gridController.WorldToGridPos(mouseWorldPos), block.GetComponent<Block>());
        if (successfullyPlaced)
        {
            availableAmount--;
            UpdatePreview();
            UpdateSelectable();
        }
        else
        {
            Destroy(block);
        }
    }

    void UpdateSelectable()
    {
        selectable.SetCount(availableAmount);
        
    }

    bool IsBlockRotateable()
    {
        if(currentBlockPrefab == null) return false;
        IRotateable rotateable = currentBlockPrefab.GetComponent<IRotateable>();
        return rotateable != null;
    }

    bool IsBlockClockWiseTypeRotate()
    {
        if(!IsBlockRotateable()) return false;
        IRotateable rotateable = currentBlockPrefab.GetComponent<IRotateable>();
        return rotateable.isClockWiseRotateable();
    }

    SpriteRenderer GetRotationDisplay()
    {
        return previewSpriteIcon.transform.GetChild(0).GetComponent<SpriteRenderer>();
        
    }
    
    
    
}
