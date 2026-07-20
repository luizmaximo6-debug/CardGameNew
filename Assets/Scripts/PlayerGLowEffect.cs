using UnityEngine;
using UnityEngine.UI;

namespace SinuousProductions
{
    public class PlayerGlowEffect : MonoBehaviour
    {
        public float speed = 2f;
        public float maxAlpha = 0.4f;

        private Image image;
        private bool pulsing = false;

        // Lista estática de todos os glows ativos
        public static System.Collections.Generic.List<PlayerGlowEffect> allGlows = 
            new System.Collections.Generic.List<PlayerGlowEffect>();

        void Awake()
        {
            image = GetComponent<Image>();
            allGlows.Add(this);
            if (image != null)
            {
                Color c = image.color;
                c.a = 0f;
                image.color = c;
            }
        }

        void OnDestroy()
        {
            allGlows.Remove(this);
        }

        public static void StopAll()
        {
            foreach (var glow in allGlows)
                if (glow != null) glow.StopGlow();
        }

        public void StartGlow() => pulsing = true;

        public void StopGlow()
        {
            pulsing = false;
            if (image != null)
            {
                Color c = image.color;
                c.a = 0f;
                image.color = c;
            }
        }

        void Update()
        {
            if (!pulsing || image == null) return;

            float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
            Color c = image.color;
            c.a = Mathf.Lerp(0f, maxAlpha, t);
            image.color = c;
        }
    }
}