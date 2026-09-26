using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockSelectable : MonoBehaviour
{
    [SerializeField] private Block block;
    [SerializeField] private int count;
    private TMP_Text textBox;
    private Button button;

    private BlockPlacer blockPlacer;
    

    private void Awake()
    {
        button = GetComponent<Button>();
        textBox = GetComponentInChildren<TMP_Text>();
        
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
        blockPlacer.SetBlock(block , count, this);
    }

    public void SetCount(int count)
    {
        this.count = count;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        textBox.text = count.ToString();
    }
}
