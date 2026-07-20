using UnityEngine;
using UnityEngine.UI;


namespace SinuousProductions
{
   public class SelectionSlotUI : MonoBehaviour
    {
        [Header("Prefabs por tipo")]
        public GameObject prefabEspada;
        public GameObject prefabEscudo;
        public GameObject prefabPoder;
        public GameObject prefabGrab;
        public GameObject prefabMeditacao;

        [Header("Undo")]
public GameObject undoButton;

        private Card assignedCard;
        private GameObject instantiatedCard;
        private CardUI originalCardUI;

        public void AssignCard(Card card, CardUI cardUI)
        {
            assignedCard = card;
            originalCardUI = cardUI;
            ClearSlotVisual();

            GameObject prefabToUse = null;

            switch (card.cardType)
            {
                case CardType.ESPADA:    prefabToUse = prefabEspada;    break;
                case CardType.ESCUDO:    prefabToUse = prefabEscudo;    break;
                case CardType.PODER:     prefabToUse = prefabPoder;     break;
                case CardType.GRAB:      prefabToUse = prefabGrab;      break;
                case CardType.MEDITACAO: prefabToUse = prefabMeditacao; break;
            }

            if (prefabToUse != null)
            {
                instantiatedCard = Instantiate(prefabToUse, transform);


                RectTransform rt = instantiatedCard.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = Vector2.zero;
                    rt.localScale = Vector3.one * 0.8f;
                }

                CardUI cardUISlot = instantiatedCard.GetComponent<CardUI>();
                if (cardUISlot != null)
                {
                    cardUISlot.cardData = card;
                    cardUISlot.isPlayerCard = false;
                     Destroy(cardUISlot); // ← remove o CardUI da cópia, não precisa dele
                }
                 if (card.cardType == CardType.PODER && TemEscudoEmSlotAnterior())
    {
        Image img = instantiatedCard.GetComponent<Image>();
        if (img != null)
            instantiatedCard.AddComponent<SlotPoderGlow>();
    }
            }
// Move UndoButton para frente da carta
if (undoButton != null)
{
    undoButton.transform.SetAsLastSibling();
}
            Debug.Log($"Slot preenchido com {card.cardName}");
        }

       

        public void ClearSlot()
        {
            assignedCard = null;
            originalCardUI = null;
            ClearSlotVisual();
        }

        void ClearSlotVisual()
{
    if (instantiatedCard != null)
    {
        DestroyImmediate(instantiatedCard);
        instantiatedCard = null;
    }
}

        public Card GetCard() => assignedCard;
        public CardUI GetCardUI() => originalCardUI;
     public void TriggerUndo()
{
    if (assignedCard == null) return;

    Card cardToRemove = assignedCard;
    CardUI cardUIToReset = originalCardUI;

    ClearSlot();

    if (cardUIToReset != null)
        cardUIToReset.ResetPosition();

    // Busca pelo card na lista em vez de usar índice do slot
    BattleManager.Instance.RemoveCardFromSelection(cardToRemove);
    BattleManager.Instance.battleController?.HideFightButton();

    Debug.Log($"Carta {cardToRemove.cardName} devolvida!");
}

int GetSlotIndex()
{
    SelectionSlotUI[] slots = BattleManager.Instance.selectionSlots;
    for (int i = 0; i < slots.Length; i++)
    {
        if (slots[i] == this) return i;
    }
    return -1;
}
bool TemEscudoEmSlotAnterior()
{
    SelectionSlotUI[] slots = BattleManager.Instance.selectionSlots;
    int meuIndex = GetSlotIndex();

    for (int i = 0; i < meuIndex; i++)
    {
        if (slots[i] != null && slots[i].GetCard() != null &&
            slots[i].GetCard().cardType == CardType.ESCUDO)
            return true;
    }
    return false;
}
    }
    
    
}
