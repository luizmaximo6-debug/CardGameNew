using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SinuousProductions
{
    [System.Serializable]
    public class AnimationConfig
    {
        public string animationName;
        public CardType playerCardType;
        public CardType npcCardType;
        public Animator animationOverlay;
        public bool flipHorizontal;
        public bool requiresPlayerMana;
        public bool requiresNPCMana;
    }

    public class BattleController : MonoBehaviour
    {
        [Header("UI")]
        public GameObject nextLevelButton;
        public GameObject retryLevelButton;
        public GameObject fightButton;

        [Header("Dark Overlay")]
        public CanvasGroup darkOverlay;

        [Header("Animation Configurations")]
        public AnimationConfig[] animationConfigs;

        [Header("Hero UIs")]
        public HeroUI playerHeroUI;
        public HeroUI npcHeroUI;

        public void OnStartBattleClicked()
        {
            int requiredCards = BattleManager.Instance.currentLevel.numberOfSlots;
            if (BattleManager.Instance.playerSelectedCards.Count < requiredCards)
            {
                Debug.Log($"Precisa selecionar {requiredCards} cartas!");
                return;
            }

            Debug.Log("=== INICIANDO BATALHA ===");
            BattleManager.Instance.StartBattle();
            StartCoroutine(RunBattle());
            HideFightButton();

            // Desabilita undo quando batalha começa
            SlotHoverArea[] hoverAreas = FindObjectsOfType<SlotHoverArea>();
            foreach (var area in hoverAreas)
                area.enabled = false;
        }

        System.Collections.IEnumerator RunBattle()
        {
            int totalRounds = BattleManager.Instance.currentLevel.numberOfSlots;

            for (int round = 0; round < totalRounds; round++)
            {
                BattleManager.Instance.currentRound = round;

                Card playerCard = BattleManager.Instance.playerSelectedCards[round];
                Card npcCard = BattleManager.Instance.npcCards[round];

                Debug.Log($"\n--- RODADA {round + 1} ---");
                Debug.Log($"Player: {playerCard.cardName} vs NPC: {npcCard.cardName}");

                int playerManaBeforeCombat = BattleManager.Instance.playerHero.currentMana;
                int npcManaBeforeCombat = BattleManager.Instance.npcHero.currentMana;
                int playerHealthBeforeCombat = BattleManager.Instance.playerHero.currentHealth;
                int npcHealthBeforeCombat = BattleManager.Instance.npcHero.currentHealth;

                CombatResult result = CombatResolver.ResolveCombat(
                    playerCard,
                    npcCard,
                    BattleManager.Instance.playerHero,
                    BattleManager.Instance.npcHero
                );
               
                Debug.Log($"Resultado: {result.description}");

               CardType playerTypeReal = (playerCard.cardType == CardType.PODER && playerManaBeforeCombat == 0)
    ? CardType.PODER_NEGRO : playerCard.cardType;

CardType npcTypeReal = (npcCard.cardType == CardType.PODER && npcManaBeforeCombat == 0)
    ? CardType.PODER_NEGRO : npcCard.cardType;

AnimationConfig matchingAnimation = FindMatchingAnimation(
    playerTypeReal,
    npcTypeReal,
    playerManaBeforeCombat,
    npcManaBeforeCombat
);

                if (matchingAnimation != null)
                {
                    Debug.Log($"[ANIMAÇÃO] TOCANDO: {matchingAnimation.animationName}");

                    float clipLength = matchingAnimation.animationOverlay
                        .runtimeAnimatorController.animationClips[0].length;

                    Debug.Log($"[ANIMAÇÃO] Duração do clip: {clipLength}s");

                    PlayAnimation(matchingAnimation.animationOverlay, matchingAnimation.flipHorizontal, clipLength);

                    yield return new WaitForSeconds(clipLength + 0.5f);
                     ManaVisualController.Instance?.AtualizarManaVisual(playerCard.cardType, npcCard.cardType);

                }

                Debug.Log($"Player toma {result.playerDamage} | NPC toma {result.npcDamage}");

                if (result.playerDamage > 0 && playerHeroUI != null)
                    yield return StartCoroutine(playerHeroUI.DamageAnimation(
                        playerHealthBeforeCombat,
                        playerHealthBeforeCombat - result.playerDamage));

                if (result.npcDamage > 0 && npcHeroUI != null)
                    yield return StartCoroutine(npcHeroUI.DamageAnimation(
                        npcHealthBeforeCombat,
                        npcHealthBeforeCombat - result.npcDamage));

                BattleManager.Instance.playerHero.currentHealth -= result.playerDamage;
                BattleManager.Instance.npcHero.currentHealth -= result.npcDamage;

                if (playerHeroUI != null)
                    playerHeroUI.UpdateHP(BattleManager.Instance.playerHero.currentHealth);
                if (npcHeroUI != null)
                    npcHeroUI.UpdateHP(BattleManager.Instance.npcHero.currentHealth);

                BattleManager.Instance.playerTotalDamage += result.npcDamage;
                BattleManager.Instance.npcTotalDamage += result.playerDamage;

                Debug.Log($"Player HP: {BattleManager.Instance.playerHero.currentHealth}");
                Debug.Log($"NPC HP: {BattleManager.Instance.npcHero.currentHealth}");

                if (BattleManager.Instance.playerHero.currentHealth <= 0)
                {
                    Debug.Log("\n*** NPC VENCEU! ***");
                    yield return new WaitForSeconds(0.5f);
                    ShowRetryButton();
                    yield break;
                }
                if (BattleManager.Instance.npcHero.currentHealth <= 0)
                {
                    Debug.Log("\n*** PLAYER VENCEU! ***");
                    yield return new WaitForSeconds(0.5f);
                    ShowVictoryButton();
                    yield break;
                }

                if (round < totalRounds - 1)
                {
                    yield return new WaitForSeconds(2f);
                }
            }

            Debug.Log("\n=== FIM DA BATALHA ===");
            yield return new WaitForSeconds(0.5f);

            if (BattleManager.Instance.currentLevel.mustKillToWin)
            {
                Debug.Log("*** TUTORIAL: Precisava zerar HP! ***");
                ShowRetryButton();
            }
            else
            {
                if (BattleManager.Instance.playerTotalDamage > BattleManager.Instance.npcTotalDamage)
                {
                    Debug.Log($"*** PLAYER VENCEU! ***");
                    ShowVictoryButton();
                }
                else if (BattleManager.Instance.npcTotalDamage > BattleManager.Instance.playerTotalDamage)
                {
                    Debug.Log($"*** NPC VENCEU! ***");
                    ShowRetryButton();
                }
                else
                {
                    Debug.Log("*** EMPATE! ***");
                    ShowRetryButton();
                }
            }
        }

        AnimationConfig FindMatchingAnimation(CardType playerType, CardType npcType, int playerMana, int npcMana)
        {
            foreach (AnimationConfig config in animationConfigs)
            {
                if (config.playerCardType == playerType && config.npcCardType == npcType)
                {
                    if (config.requiresPlayerMana && playerMana < 1) continue;
                    if (config.requiresNPCMana && npcMana < 1) continue;
                    return config;
                }
            }
            return null;
        }
void ShowVictoryButton()
{
    HideFightButton();

    // Se é o último nível, toca fireworks em vez de mostrar botão
    if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
    {
        FireworksController.Instance?.PlayFireworks();
        return;
    }

    Debug.Log("[BOTÃO] Tentando mostrar botão PRÓXIMA FASE");
    if (nextLevelButton != null)
    {
        nextLevelButton.SetActive(true);
        nextLevelButton.GetComponentInChildren<ScalePulseEffect>()?.StartPulse();
        Debug.Log("[BOTÃO] Botão PRÓXIMA FASE ativado!");
    }
    else
    {
        Debug.LogError("[BOTÃO] NextLevelButton é NULL!");
    }
}

        void ShowRetryButton()
        {
            HideFightButton();
            Debug.Log("[BOTÃO] Tentando mostrar botão TENTAR NOVAMENTE");
            if (retryLevelButton != null)
            {
                retryLevelButton.SetActive(true);
                retryLevelButton.GetComponentInChildren<ScalePulseEffect>()?.StartPulse();
                Debug.Log("[BOTÃO] Botão TENTAR NOVAMENTE ativado!");
            }
            else
            {
                Debug.LogError("[BOTÃO] RetryLevelButton é NULL!");
            }
        }

        void PlayAnimation(Animator animator, bool flipHorizontal, float clipLength)
        {
            if (animator != null)
            {
                Vector3 scale = animator.transform.localScale;
                scale.x = flipHorizontal ? -1f : 1f;
                scale.y = 1f;
                scale.z = 1f;
                animator.transform.localScale = scale;

                if (darkOverlay != null)
                {
                    darkOverlay.gameObject.SetActive(true);
                    StartCoroutine(FadeIn(darkOverlay, 0.2f));
                }

                animator.gameObject.SetActive(true);
                animator.enabled = true;
                animator.Rebind();
                animator.Play(0);
                StartCoroutine(HideAnimationAfterDelay(animator, clipLength));
            }
        }

        System.Collections.IEnumerator HideAnimationAfterDelay(Animator animator, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (darkOverlay != null)
                StartCoroutine(FadeOut(darkOverlay, 0.2f));

            if (animator != null)
                animator.gameObject.SetActive(false);
        }

        System.Collections.IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        System.Collections.IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }

        public void HideFightButton()
        {
            if (fightButton != null)
            {
                Transform fightIcon = fightButton.transform.Find("FightIcon");
                if (fightIcon != null)
                    fightIcon.gameObject.SetActive(false);

                Button fightBtn = fightButton.GetComponent<Button>();
                if (fightBtn != null) fightBtn.enabled = false;
            }
        }

        public void ShowFightButton()
        {
            if (fightButton != null)
            {
                Transform fightIcon = fightButton.transform.Find("FightIcon");
                Debug.Log($"[FIGHT] fightButton: {fightButton.name}");
                Debug.Log($"[FIGHT] fightIcon encontrado: {fightIcon != null}");

                if (fightIcon != null)
                {
                    fightIcon.gameObject.SetActive(true);
                    fightIcon.GetComponentInChildren<ScalePulseEffect>()?.StartPulse();
                    Debug.Log("[FIGHT] FightIcon ativado!");
                }

                Button fightBtn = fightButton.GetComponent<Button>();
                if (fightBtn != null) fightBtn.enabled = true;
            }
            else
            {
                Debug.LogError("[FIGHT] fightButton é NULL!");
            }
        }

        public void LoadNextLevel()
        {
                ManaVisualController.Instance?.HideAll();

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Registra progresso (buildIndex + 2 porque índice começa em 0)
    ProgressoUI.RegistrarProgresso(nextSceneIndex + 1);

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextSceneIndex);
            else
                Debug.Log("Você completou todos os níveis!");
        }

        public void RetryLevel()
        {
             ManaVisualController.Instance?.HideAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}