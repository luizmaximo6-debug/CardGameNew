using UnityEngine;

namespace SinuousProductions
{
    public class ScalePulseEffect : MonoBehaviour
    {
        public float speed = 2f;
        public float minScale = 0.95f;
        public float maxScale = 1.05f;

        private Vector3 originalScale;
        private bool pulsing = false;

        void Awake()
        {
            originalScale = transform.localScale;
        }

        public void StartPulse()
        {
            pulsing = true;
        }

        public void StopPulse()
        {
            pulsing = false;
            transform.localScale = originalScale;
        }

        void Update()
        {
            if (!pulsing) return;

            float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
            float scale = Mathf.Lerp(minScale, maxScale, t);
            transform.localScale = originalScale * scale;
        }
    }
}