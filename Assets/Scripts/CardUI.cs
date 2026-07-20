using UnityEngine;
using UnityEngine.EventSystems;

namespace SinuousProductions
{
    public class CardUI : MonoBehaviour, IPointerClickHandler
    {
        public Card cardData;
        public bool isPlayerCard = true;
        
        private Vector3 originalLocalPosition;
        private Transform originalParent;
        private bool initialized = false;
        private int originalSiblingIndex;
        
        void Start()
        {
            if (!initialized)
            {
                originalLocalPosition = transform.localPosition;
                originalParent = transform.parent;
                originalSiblingIndex = transform.parent.GetSiblingIndex();
                initialized = true;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"[CLICK] Carta: {cardData.cardName} | isPlayerCard: {isPlayerCard} | CanSelect: {BattleManager.Instance.CanSelectCard()}");
    
            if (!isPlayerCard) return;
            
            if (!BattleManager.Instance.CanSelectCard())
            {
                Debug.Log("Já selecionou todas as cartas!");
                return;
            }
            
            BattleManager.Instance.AddCardToSelection(cardData, this);

            // Cria placeholder para manter espaço
            CardHover hover = GetComponentInParent<CardHover>(true);
            if (hover != null)
                hover.CreateSelectionPlaceholder();

            transform.parent.gameObject.SetActive(false);
            
            Debug.Log($"Carta {cardData.cardName} selecionada!");
        }
        
        public void ResetPosition()
        {
            CardHover hover = GetComponentInParent<CardHover>(true);
            if (hover != null)
                hover.ForceReturnToContainer();

            transform.SetParent(originalParent);
            transform.parent.SetSiblingIndex(originalSiblingIndex);
            transform.localPosition = originalLocalPosition;
            transform.localScale = Vector3.one;
            transform.parent.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
    }
}