using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles knight selection from a list of available sprite sets in the main menu.
/// Displayes a preview and stores the selected set for use in the jousting game.
/// </summary>
public class KnightSelector : MonoBehaviour
{
    [Header("UI Components")]
    public Image knightPreviewImage;      // The UI image showing the current selection
    public Button leftArrow;
    public Button rightArrow;

    [Header("Knight Options")]
    public List<KnightSpriteSet> knightOptions;     // Where knight sprites are assignes

    private int currentIndex = 0;
    public static KnightSpriteSet SelectedKnightSet { get; private set; }       // Accessible by JoustGameManager
    // Start is called before the first frame update
    void Start()
    {
        // Button listeners
        leftArrow.onClick.AddListener(SelectPreviousKnight);
        rightArrow.onClick.AddListener(SelectNextKnight);

        UpdateKnightPreview();
    }

    public void SelectPreviousKnight()
    {
        currentIndex = (currentIndex - 1 + knightOptions.Count) % knightOptions.Count;
        UpdateKnightPreview();
    }

    public void SelectNextKnight()
    {
        currentIndex = (currentIndex + 1) % knightOptions.Count;
        UpdateKnightPreview();
    }

    private void UpdateKnightPreview()
    {
        if (knightOptions.Count == 0) return;

        SelectedKnightSet = knightOptions[currentIndex];
        knightPreviewImage.sprite = SelectedKnightSet.idleSide1;        // Show Idle Side 1 as preview
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
