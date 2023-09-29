using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LayoutGapHandler : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    // Fields for controlling the horizontal layout of elements.
    [SerializeField] float gapOnSides = 0f;       // The gap on the sides of the horizontal layout group.
    [SerializeField] float rightBoundry;          // The right boundry of the layout group.
    [SerializeField] float elementWidth = 100.0f; // Width of the layout elements.
    [SerializeField] int layoutElementsNum;       // The number of layout elements attached to this obj. 
    [SerializeField] float visibleElementNum;     // Number of layout elements can be shown on the panel at the same time.
    [SerializeField] float spacing;               // Spacing property of the horizontal layout group.
    [SerializeField] float panelWidth;            // The total width of the stuff on the panel including the layout elements and the gapOnSides.

    // References.
    RectTransform rectTransform;
    HorizontalLayoutGroup horizontalLayoutGroup;
    #endregion


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS
    private void Start()
    {
        // Initialize the fields.
        rectTransform = GetComponent<RectTransform>();
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        spacing = horizontalLayoutGroup.spacing;
    }

    private void Update()
    {
        // Get the number of layout elements.
        layoutElementsNum = rectTransform.childCount;

        // Calculate the width of the panel.
        panelWidth = (layoutElementsNum * elementWidth + gapOnSides * 2 + (spacing * layoutElementsNum - 1)) * rectTransform.localScale.x;

        // Calculate the right c of the layout group.
        rightBoundry = panelWidth - visibleElementNum * elementWidth - gapOnSides;

        // The left panelWidth is simply the "gapOnSides", the right boundry is the 
        // "rightBoundry" which will create a gap almost identical to the "gapOnSides"
        // on the right side.
        if (rectTransform.localPosition.x > gapOnSides)
            rectTransform.localPosition = new Vector3(gapOnSides, rectTransform.localPosition.y, rectTransform.localPosition.z);
        else if (rectTransform.localPosition.x < -rightBoundry)
            rectTransform.localPosition = new Vector3(-rightBoundry, rectTransform.localPosition.y, rectTransform.localPosition.z);
    }
    #endregion
}

