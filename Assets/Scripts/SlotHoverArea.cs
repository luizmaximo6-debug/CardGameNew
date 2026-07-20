using UnityEngine;
using UnityEngine.EventSystems;

namespace SinuousProductions
{
    public class SlotHoverArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject undoButton;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GetComponent<SelectionSlotUI>()?.GetCard() != null)
                undoButton?.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            undoButton?.SetActive(false);
        }
    }
}