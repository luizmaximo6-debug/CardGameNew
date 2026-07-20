using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace SinuousProductions
{
    public class BattleIntroController : MonoBehaviour
    {
        [Header("Title")]
        public GameObject gameTitle;

        [Header("Overlay")]
        public CanvasGroup darkOverlayIntro;

        [Header("Overlay Total")]
        public GameObject darkOverlayTotal;

        [Header("Hero")]
        public Animator heroTailAnimator;
        public GameObject fogoStatic;

        [Header("NPC Frames")]
        public GameObject npcFrame1;
        public GameObject npcFrame2;
        public float frameDuration = 1.5f;

        [Header("NPC")]
        public RectTransform npcContainer;
        public List<RectTransform> npcCards;

        [Header("Play Button")]
        public GameObject playButton;

        [Header("Player Cards")]
        public List<PlayerGlowEffect> playerGlows;

        [Header("Settings")]
        public float cardSlideOffset = 300f;
        public float cardSlideDuration = 0.4f;
        public float timeBetweenCards = 0.3f;
        public float pauseAfterCards = 1f;
        public float overlayFadeDuration = 0.5f;

        void Start()
        {
              // Se é o Level 1, reseta o título
    if (SceneManager.GetActiveScene().buildIndex == 0)
        PlayerPrefs.DeleteKey("TituloMostrado");

    // Só mostra o título na primeira vez
    if (gameTitle != null)
    {
        bool primeiraVez = PlayerPrefs.GetInt("TituloMostrado", 0) == 0;
        gameTitle.SetActive(primeiraVez);
    }
            if (heroTailAnimator != null)
            {
                heroTailAnimator.enabled = false;
                heroTailAnimator.gameObject.SetActive(false);
            }

            if (npcFrame2 != null)
                npcFrame2.SetActive(false);

            foreach (var card in npcCards)
            {
                if (card != null)
                {
                    CanvasGroup cg = card.GetComponent<CanvasGroup>();
                    if (cg == null) cg = card.gameObject.AddComponent<CanvasGroup>();
                    cg.alpha = 0f;
                }
            }

            if (darkOverlayIntro != null)
            {
                darkOverlayIntro.gameObject.SetActive(true);
                darkOverlayIntro.alpha = 1f;
            }

            if (playButton != null)
                playButton.SetActive(true);

            // Só mostra o título na primeira vez
            if (gameTitle != null)
            {
                bool primeiraVez = PlayerPrefs.GetInt("TituloMostrado", 0) == 0;
                gameTitle.SetActive(primeiraVez);
            }
        }

        public void OnPlayButtonClicked()
        {
            PlayerPrefs.SetInt("TituloMostrado", 1);
            PlayerPrefs.Save();

            if (gameTitle != null)
                gameTitle.SetActive(false);

            if (playButton != null)
                playButton.SetActive(false);

            if (darkOverlayTotal != null)
                darkOverlayTotal.SetActive(false);

            StartCoroutine(NPCFrameAnimation());
            StartCoroutine(RunIntroSequence());
        }

        IEnumerator NPCFrameAnimation()
        {
            for (int i = 0; i < 2; i++)
            {
                if (npcFrame1 != null) npcFrame1.SetActive(false);
                if (npcFrame2 != null) npcFrame2.SetActive(true);
                yield return new WaitForSeconds(frameDuration);

                if (npcFrame2 != null) npcFrame2.SetActive(false);
                if (npcFrame1 != null) npcFrame1.SetActive(true);
                yield return new WaitForSeconds(frameDuration);
            }
            if (npcFrame2 != null) npcFrame2.SetActive(false);
            if (npcFrame1 != null) npcFrame1.SetActive(true);
        }

        IEnumerator RunIntroSequence()
        {
            foreach (var card in npcCards)
            {
                if (card == null) continue;

                CanvasGroup cg = card.GetComponent<CanvasGroup>();
                if (cg == null) continue;

                Vector3 originalPos = card.anchoredPosition3D;
                Vector3 startPos = originalPos + new Vector3(cardSlideOffset, 0, 0);
                card.anchoredPosition3D = startPos;

                yield return StartCoroutine(SlideCard(card, cg, startPos, originalPos));
                yield return new WaitForSeconds(timeBetweenCards);
            }

            yield return new WaitForSeconds(pauseAfterCards);

            yield return StartCoroutine(FadeOut(darkOverlayIntro, overlayFadeDuration));

            darkOverlayIntro.gameObject.SetActive(false);

            if (fogoStatic != null)
                fogoStatic.SetActive(false);

            if (heroTailAnimator != null)
            {
                heroTailAnimator.gameObject.SetActive(true);
                heroTailAnimator.enabled = true;
            }

            foreach (var glow in playerGlows)
            {
                if (glow != null)
                    glow.StartGlow();
            }
        }

        IEnumerator SlideCard(RectTransform card, CanvasGroup cg, Vector3 from, Vector3 to)
        {
            float elapsed = 0f;
            while (elapsed < cardSlideDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / cardSlideDuration;
                float smooth = Mathf.SmoothStep(0f, 1f, t);
                card.anchoredPosition3D = Vector3.Lerp(from, to, smooth);
                cg.alpha = Mathf.Lerp(0f, 1f, smooth);
                yield return null;
            }
            card.anchoredPosition3D = to;
            cg.alpha = 1f;
        }

        IEnumerator FadeOut(CanvasGroup cg, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                cg.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                yield return null;
            }
            cg.alpha = 0f;
        }

        public void StopAllGlows()
        {
            PlayerGlowEffect.StopAll();
        }
    }
}