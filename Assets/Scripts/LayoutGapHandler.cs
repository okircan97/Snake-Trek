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

    // Fields for controlling the horizontal layout of elements.
    [SerializeField] float gapOnSides = 0f;       // The gap on the sides of the horizontal layout group.
    [SerializeField] float rightBoundry;          // The right boundry of the layout group.
    [SerializeField] float elementWidth = 100.0f; // Width of the layout elements.
    [SerializeField] int layoutElementsNum;       // The number of layout elements attached to this obj. 
    [SerializeField] float visibleElementNum;     // Number of layout elements can be shown on the panel at the same time.
    [SerializeField] float spacing;               // Spacing property of the horizontal layout group.
    [SerializeField] float panelWidth;            // The total width of the stuff on the panel including the layout elements and the gapOnSides.

    // References.
    RectTransform rt;
    HorizontalLayoutGroup hlg;


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////

    private void Start()
    {
        // Initialize references.
        rt = GetComponent<RectTransform>();
        hlg = GetComponent<HorizontalLayoutGroup>();
        spacing = hlg.spacing;

        // Two and almost half of a button is visible.
        visibleElementNum = 2.4f;
    }

    private void Update()
    {
        // Get the number of layout elements.
        layoutElementsNum = rt.childCount;

        // Calculate the width of the panel.
        panelWidth = (layoutElementsNum * elementWidth + gapOnSides * 2 + (spacing * layoutElementsNum - 1)) * rt.localScale.x;

        // Calculate the right c of the layout group.
        rightBoundry = panelWidth - visibleElementNum * elementWidth - gapOnSides;

        // The left panelWidth is simply the "gapOnSides", the right boundry is the 
        // "rightBoundry" which will create a gap almost identical to the "gapOnSides"
        // on the right side.
        if (rt.localPosition.x > gapOnSides)
            rt.localPosition = new Vector3(gapOnSides, rt.localPosition.y, rt.localPosition.z);
        else if (rt.localPosition.x < -rightBoundry)
            rt.localPosition = new Vector3(-rightBoundry, rt.localPosition.y, rt.localPosition.z);
    }
}

