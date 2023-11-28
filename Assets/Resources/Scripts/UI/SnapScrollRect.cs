using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SnapScrollRect : MonoBehaviour, IEndDragHandler
{
    public ScrollRect scrollRect;
    public int panelCount;

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = scrollRect.horizontalNormalizedPosition;
        int closestPanelIndex = Mathf.RoundToInt(percentage * (panelCount - 1));
        float newPercentage = (float)closestPanelIndex / (panelCount - 1);
        scrollRect.horizontalNormalizedPosition = newPercentage;
    }
}