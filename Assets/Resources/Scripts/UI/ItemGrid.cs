using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemGrid : MonoBehaviour
{
    // Assuming you have a reference to your Scroll Rect and Grid Layout Group
    public ScrollRect scrollRect;
    public GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        // Call this method whenever you add or remove an item
        UpdateContentSize();
    }


    // Call this method whenever you add or remove an item
    public void UpdateContentSize()
    {
        // Calculate the total height of all items
        float totalHeight = gridLayoutGroup.cellSize.y * (int)Math.Ceiling(gridLayoutGroup.transform.childCount / 5.0f);

        // Add the spacing for all rows
        totalHeight += gridLayoutGroup.spacing.y * ((int)Math.Ceiling(gridLayoutGroup.transform.childCount / 5.0f) - 1);

        // Add the padding at the top and bottom
        totalHeight += gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

        // Set the height of the content
        gridLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(gridLayoutGroup.GetComponent<RectTransform>().sizeDelta.x, totalHeight);

        // Update the Scroll Rect
        scrollRect.content = gridLayoutGroup.GetComponent<RectTransform>();
    }

}
