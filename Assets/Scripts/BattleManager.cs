using UnityEngine;
using System.Collections.Generic;

namespace SinuousProductions
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;
        
        [Header("Level Configuration")]
        public LevelData currentLevel;
        
        [Header("Heroes")]
        public Hero playerHero;
        public Hero npcHero;
        
        [Header("Player Deck")]
        public List<Card> playerSelectedCards = new List<Card>();
        
        [Header("NPC Deck")]
        public List<Card> npcCards = new List<Card>();
        
        [Header("Selection Slots UI")]
        public SelectionSlotUI[] selectionSlots = new SelectionSlotUI[5];
        
        [Header("Battle State")]
        public int currentRound = 0;
        public int playerTotalDamage = 0;
        public int npcTotalDamage = 0;

        [Header("Controllers")]
        public BattleController battleController;
        
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
                
            if (currentLevel != null)
                SetupLevel(currentLevel);
            else
                Debug.LogError("Nenhum Level configurado no BattleManager!");
        }
        
        public void SetupLevel(LevelData level)
        {
            Debug.Log($"Carregando {level.levelName}...");
            
            playerHero = level.playerHero;
            npcHero = level.npcHero;

            if (playerHero != null)
            {
                playerHero.currentHealth = level.playerStartHP;
                playerHero.currentMana = 0;
                playerHero.currentXP = 0;
            }

            if (npcHero != null)
            {
                npcHero.currentHealth = level.npcStartHP;
                npcHero.currentMana = 0;
                npcHero.currentXP = 0;
            }
            
            npcCards = new List<Card>(level.npcCards);
            
            if (level.shuffleNPC)
                ShuffleNPCCards();
            else
                Debug.Log("NPC cards em ordem fixa (tutorial mode)");
            
            Debug.Log($"Level configurado: {level.numberOfSlots} slots");
        }
        
        void ShuffleNPCCards()
        {
            for (int i = npcCards.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                Card temp = npcCards[i];
                npcCards[i] = npcCards[randomIndex];
                npcCards[randomIndex] = temp;
            }
            Debug.Log("Cartas do NPC embaralhadas!");
        }
        
        public void StartBattle()
        {
            Debug.Log("Batalha iniciada!");
            currentRound = 0;
            playerTotalDamage = 0;
            npcTotalDamage = 0;
        }
        
        public bool CanSelectCard()
        {
            int maxSlots = currentLevel != null ? currentLevel.numberOfSlots : 5;
            return playerSelectedCards.Count < maxSlots;
        }
        
        public void AddCardToSelection(Card card, CardUI cardUI)
        {
           if (CanSelectCard())
{
    int maxSlots = currentLevel != null ? currentLevel.numberOfSlots : 5;
    playerSelectedCards.Add(card);
    PlayerGlowEffect.StopAll();

    // Encontra primeiro slot vazio
    int slotIndex = -1;
    for (int i = 0; i < selectionSlots.Length; i++)
    {
        if (selectionSlots[i] != null && selectionSlots[i].GetCard() == null)
        {
            slotIndex = i;
            break;
        }
    }

    if (slotIndex >= 0)
        selectionSlots[slotIndex].AssignCard(card, cardUI);

    Debug.Log($"Carta {card.cardName} adicionada! Total: {playerSelectedCards.Count}/{maxSlots}");

    if (playerSelectedCards.Count == maxSlots)
    {
        if (battleController != null)
            battleController.ShowFightButton();
    }
}
        }

        public void RemoveCardAndReorganize(Card card)
        {
            int index = playerSelectedCards.IndexOf(card);
            if (index < 0) return;

            playerSelectedCards.RemoveAt(index);

            // Esconde fight button
            if (battleController != null)
                battleController.HideFightButton();

            // Reorganiza slots
            for (int i = index; i < selectionSlots.Length; i++)
            {
                if (i + 1 < selectionSlots.Length && selectionSlots[i + 1] != null)
                {
                    Card nextCard = selectionSlots[i + 1].GetCard();
                    CardUI nextCardUI = selectionSlots[i + 1].GetCardUI();

                    if (nextCard != null)
                    {
                        selectionSlots[i].AssignCard(nextCard, nextCardUI);
                        selectionSlots[i + 1].ClearSlot();
                    }
                    else
                    {
                        selectionSlots[i].ClearSlot();
                        break;
                    }
                }
                else
                {
                    selectionSlots[i].ClearSlot();
                    break;
                }
            }

            Debug.Log($"Carta removida! Total: {playerSelectedCards.Count}");
        }
        
        public void RemoveCardFromSelection(Card card)
        {
            playerSelectedCards.Remove(card);
            Debug.Log($"Carta {card.cardName} removida! Total: {playerSelectedCards.Count}");
        }
        public void RemoveCardAtIndex(int index)
{
    if (index < 0 || index >= playerSelectedCards.Count) return;

    playerSelectedCards.RemoveAt(index);

    if (battleController != null)
        battleController.HideFightButton();

    // Limpa só o slot clicado, sem reorganizar
    if (index < selectionSlots.Length && selectionSlots[index] != null)
        selectionSlots[index].ClearSlot();

    Debug.Log($"Carta removida no índice {index}! Total: {playerSelectedCards.Count}");
}
    }
}