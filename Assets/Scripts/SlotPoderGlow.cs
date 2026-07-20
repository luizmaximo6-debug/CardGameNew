using UnityEngine;
using UnityEngine.UI;

namespace SinuousProductions
{
    public class SlotPoderGlow : MonoBehaviour
    {
        public float speed = 2f;

        private Image image;
        private Color originalColor;

        void Awake()
        {
            image = GetComponent<Image>();
            if (image != null)
                originalColor = image.color;
        }

        void Update()
        {
            if (image == null) return;
            float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
            image.color = Color.Lerp(originalColor, Color.white, t);
        }
    }
}