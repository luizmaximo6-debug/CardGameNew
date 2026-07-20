using UnityEngine;

namespace SinuousProductions
{
    [CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
    public class Hero : ScriptableObject
    {
        public string heroName;
        
        // Atributos do herói
        public int attackPower;     // X
        public int magicPower;      // Y
        public int health;          // Z
        
        // Recursos
       [System.NonSerialized]
public int currentHealth;
[System.NonSerialized]
public int currentMana;
[System.NonSerialized]
public int currentXP;
        
        // Deck de 10 cartas (2 de cada tipo)
        public Card[] deck = new Card[10];
        
        // Método para inicializar recursos no começo da batalha
        public void InitializeBattle()
        {
            currentHealth = health;
            currentMana = 0;
            currentXP = 0;
        }
    }
}