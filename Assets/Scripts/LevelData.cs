using UnityEngine;

namespace SinuousProductions
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        public int levelNumber = 1;
        public string levelName = "Tutorial 1";
        
        [Header("Battle Setup")]
        public int numberOfSlots = 3;
        
        [Header("NPC Configuration")]
        public Card[] npcCards;
        public bool shuffleNPC = false;
        public Hero npcHero;
        
        [Header("Player Configuration")]
        public Card[] playerDeck;
        public Hero playerHero;

        [Header("HP Configuration")]
        public int playerStartHP = 6;
        public int npcStartHP = 7;

        [Header("Victory Condition")]
        public bool mustKillToWin = true; // true = precisa zerar HP, false = regra de dano
    }
}