using UnityEngine;

namespace SinuousProductions
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public CardType cardType;
    }


public enum CardType
{
    ESPADA,
    ESCUDO,
    PODER,
    GRAB,
    MEDITACAO,
    PODER_NEGRO  // NOVO!
}}