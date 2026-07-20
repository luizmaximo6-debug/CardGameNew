using UnityEngine;
using TMPro;
using System.Collections;

namespace SinuousProductions
{
    public class HeroUI : MonoBehaviour
    {
        [Header("Hero")]
        public Hero hero;

        [Header("Textos")]
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI manaText;
        public TextMeshProUGUI hpDamageText;

        [Header("Coluna que treme")]
        public RectTransform heroColuna;

        [Header("Frames")]
        public GameObject frameNormal;
        public GameObject frameDano;

        private int lastHealth;
        private Vector3 hpTextOriginalPos;
        private Vector3 hpDamageTextOriginalPos;

        void Start()
        {
            if (hero != null)
            {
                lastHealth = hero.currentHealth;
                if (hpText != null)
                    hpText.text = hero.currentHealth.ToString();
            }

            if (hpText != null)
                hpTextOriginalPos = hpText.rectTransform.anchoredPosition3D;

            if (hpDamageText != null)
            {
                hpDamageTextOriginalPos = hpDamageText.rectTransform.anchoredPosition3D;
                hpDamageText.gameObject.SetActive(false);
            }

            // Garante estado inicial correto
            if (frameNormal != null) frameNormal.SetActive(true);
            if (frameDano != null) frameDano.SetActive(false);
        }

        void Update()
        {
            if (hero == null) return;
            if (manaText != null)
                manaText.text = hero.currentMana.ToString();
        }

        public void UpdateHP(int newHP)
        {
            lastHealth = newHP;
            if (hpText != null)
                hpText.text = newHP.ToString();
        }

        public IEnumerator DamageAnimation(int oldHP, int newHP)
        {
            // 1. Treme a coluna + mostra frame de dano
            if (frameNormal != null) frameNormal.SetActive(false);
            if (frameDano != null) frameDano.SetActive(true);

            if (heroColuna != null)
                yield return StartCoroutine(ShakeColumn(0.3f, 15f));

            // Espera 1 segundo com frame de dano
            yield return new WaitForSeconds(0.1f);

            // Volta ao frame normal
            if (frameDano != null) frameDano.SetActive(false);
            if (frameNormal != null) frameNormal.SetActive(true);

            // 2. Número antigo cai com fade out (0.5s)
            if (hpDamageText != null)
            {
                hpDamageText.text = oldHP.ToString();
                hpDamageText.color = new Color(1f, 0.2f, 0.2f, 1f);
                hpDamageText.rectTransform.anchoredPosition3D = hpDamageTextOriginalPos;
                hpDamageText.gameObject.SetActive(true);

                Vector3 endPos = hpDamageTextOriginalPos + new Vector3(0, -30f, 0);
                float elapsed = 0f;
                float duration = 0.5f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / duration;
                    hpDamageText.rectTransform.anchoredPosition3D = Vector3.Lerp(hpDamageTextOriginalPos, endPos, t);
                    Color c = hpDamageText.color;
                    c.a = Mathf.Lerp(1f, 0f, t);
                    hpDamageText.color = c;
                    yield return null;
                }

                hpDamageText.gameObject.SetActive(false);
                hpDamageText.rectTransform.anchoredPosition3D = hpDamageTextOriginalPos;
            }

            // 3. Número novo sobe (0.3s)
            if (hpText != null)
            {
                Vector3 startPos = hpTextOriginalPos + new Vector3(0, -20f, 0);
                Color originalColor = hpText.color;
                hpText.color = new Color(1f, 0.4f, 0.4f, 1f);

                float elapsed = 0f;
                float duration = 0.3f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / duration;
                    hpText.rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, hpTextOriginalPos, t);
                    yield return null;
                }

                hpText.rectTransform.anchoredPosition3D = hpTextOriginalPos;
                hpText.color = originalColor;
            }
        }

        IEnumerator ShakeColumn(float duration, float magnitude)
        {
            Vector3 originalPos = heroColuna.anchoredPosition3D;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float x = Random.Range(-magnitude, magnitude);
                heroColuna.anchoredPosition3D = originalPos + new Vector3(x, 0, 0);
                yield return null;
            }

            heroColuna.anchoredPosition3D = originalPos;
        }
    }
}