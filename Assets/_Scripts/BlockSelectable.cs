using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockSelectable : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private int count;
    private TMP_Text textBox;
    private Button button;
    private BlockPlacer blockPlacer;
    
    [SerializeField] Color greyedOutColor = new Color32(200, 200, 200, 200);
    Image image;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        textBox = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
        blockPlacer = FindAnyObjectByType<BlockPlacer>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(UpdateBlockPlacer);
        
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(UpdateBlockPlacer);
    }

    private void Start()
    {
        UpdateDisplay();
    }

    void UpdateBlockPlacer()
    {
        blockPlacer.SetBlock(blockPrefab , count, this);
    }

    public void SetCount(int count)
    {
        this.count = count;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        textBox.text = count.ToString();
        image.color = blockPrefab.GetComponent<Block>().TileColor;
        if(count <= 0) image.color = greyedOutColor;
    }
}
