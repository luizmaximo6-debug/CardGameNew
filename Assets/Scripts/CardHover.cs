using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SinuousProductions
{
    public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float hoverOffset = 20f;

        private bool isHovered = false;
        private Transform originalParent;
        private int originalSiblingIndex;
        private Vector3 originalWorldPosition;
        private GameObject placeholder;

        void Start()
        {
            originalParent = transform.parent;
            originalSiblingIndex = transform.GetSiblingIndex();
        }

        public void CreateSelectionPlaceholder()
        {
            if (placeholder != null) return;

            originalSiblingIndex = transform.GetSiblingIndex();

            placeholder = new GameObject("Placeholder");
            placeholder.transform.SetParent(originalParent, false);
            placeholder.transform.SetSiblingIndex(originalSiblingIndex);

            RectTransform rt = placeholder.AddComponent<RectTransform>();
            RectTransform myRt = GetComponent<RectTransform>();
            rt.sizeDelta = myRt.sizeDelta;

            Image img = placeholder.AddComponent<Image>();
            img.color = new Color(0, 0, 0, 0);
        }

        public void ForceReturnToContainer()
        {
            isHovered = false;

            if (placeholder != null)
            {
                // Volta para o lugar do placeholder
                int placeholderIndex = placeholder.transform.GetSiblingIndex();
                DestroyImmediate(placeholder);
                placeholder = null;

                if (transform.parent != originalParent)
                {
                    transform.SetParent(originalParent, false);
                    transform.SetSiblingIndex(placeholderIndex);
                }
            }
            else
            {
                if (transform.parent != originalParent)
                {
                    transform.SetParent(originalParent, false);
                    transform.SetSiblingIndex(originalSiblingIndex);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isHovered) return;
            isHovered = true;

            originalSiblingIndex = transform.GetSiblingIndex();
            originalWorldPosition = transform.position;

            placeholder = new GameObject("Placeholder");
            placeholder.transform.SetParent(originalParent, false);
            placeholder.transform.SetSiblingIndex(originalSiblingIndex);

            RectTransform rt = placeholder.AddComponent<RectTransform>();
            RectTransform myRt = GetComponent<RectTransform>();
            rt.sizeDelta = myRt.sizeDelta;

            Image img = placeholder.AddComponent<Image>();
            img.color = new Color(0, 0, 0, 0);

            Transform canvas = transform.root;
            transform.SetParent(canvas, true);
            transform.SetAsLastSibling();
            transform.position = originalWorldPosition + new Vector3(0, hoverOffset, 0);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isHovered) return;
            isHovered = false;

            transform.SetParent(originalParent, false);
            transform.SetSiblingIndex(originalSiblingIndex);

            if (placeholder != null)
                Destroy(placeholder);
        }
    }
}