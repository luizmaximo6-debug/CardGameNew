using UnityEngine;

namespace SinuousProductions
{
    public class CombatResolver
    {
        public static CombatResult ResolveCombat(Card playerCard, Card npcCard, Hero player, Hero npc)
        {
            CombatResult result = new CombatResult();
            
            CardType playerType = playerCard.cardType;
            CardType npcType = npcCard.cardType;
            
            bool playerPowerIsNegro = (playerType == CardType.PODER && player.currentMana == 0);
            bool npcPowerIsNegro = (npcType == CardType.PODER && npc.currentMana == 0);
            
            if (playerPowerIsNegro) playerType = CardType.PODER_NEGRO;
            if (npcPowerIsNegro) npcType = CardType.PODER_NEGRO;
            
            Debug.Log($"COMBATE: Player {playerType} vs NPC {npcType}");
            
            result = ResolveMatchup(playerType, npcType, player, npc);
            
            return result;
        }
        
        static CombatResult ResolveMatchup(CardType playerCard, CardType npcCard, Hero player, Hero npc)
        {
            CombatResult result = new CombatResult();
            
            // === ESCUDO ===
            if (playerCard == CardType.ESCUDO)
            {
                player.currentMana += 1;
                result.description = "ESCUDO +1 mana!";
                
                if (npcCard == CardType.ESCUDO)
                {
                    npc.currentMana += 1;
                    result.description = "Ambos ESCUDOS +1 mana - Empate!";
                    return result;
                }
                else if (npcCard == CardType.ESPADA)
                {
                    result.description += " E bloqueou ESPADA!";
                }
                else if (npcCard == CardType.GRAB)
                {
                    result.playerDamage = Mathf.CeilToInt(npc.attackPower / 2f);
                    result.description += $" Mas GRAB causa {result.playerDamage}!";
                }
                else if (npcCard == CardType.MEDITACAO)
                {
                    player.currentMana = Mathf.Max(0, player.currentMana - 1);
                    result.description = "MEDITAÇÃO roubou mana do ESCUDO!";
                }
                else if (npcCard == CardType.PODER)
                {
                    result.playerDamage = Mathf.CeilToInt(npc.magicPower / 2f);
                    result.description += $" Mas bloqueou só metade! Toma {result.playerDamage}!";
                }
                else if (npcCard == CardType.PODER_NEGRO)
                {
                    result.description += " E bloqueou PODER NEGRO!";
                }
                
                return result;
            }
            
            // Se NPC jogou ESCUDO (e player não jogou ESCUDO)
            if (npcCard == CardType.ESCUDO)
            {
                npc.currentMana += 1;
            }
            
            // EMPATES
            if (playerCard == npcCard && playerCard != CardType.PODER && playerCard != CardType.PODER_NEGRO)
            {
                result.description = "Empate!";
                return result;
            }
            
            // === ESPADA ===
            if (playerCard == CardType.ESPADA)
            {
                if (npcCard == CardType.GRAB || npcCard == CardType.MEDITACAO || npcCard == CardType.PODER_NEGRO)
                {
                    result.npcDamage = player.attackPower;
                    result.description = $"ESPADA causa {player.attackPower}!";
                }
                else if (npcCard == CardType.PODER)
                {
                    result.playerDamage = npc.magicPower;
                    result.description = $"PODER vence! Toma {npc.magicPower}!";
                }
                else if (npcCard == CardType.ESCUDO)
                {
                    result.description = "ESCUDO bloqueou ESPADA!";
                }
            }
            
            // === PODER ===
            else if (playerCard == CardType.PODER)
            {
                player.currentMana -= 1;
                
                if (npcCard == CardType.ESPADA || npcCard == CardType.MEDITACAO)
                {
                    result.npcDamage = player.magicPower;
                    result.description = $"PODER causa {player.magicPower}!";
                }
                else if (npcCard == CardType.GRAB)
                {
                    result.playerDamage = player.magicPower;
                    result.description = $"GRAB refletiu! Toma {player.magicPower}!";
                }
                else if (npcCard == CardType.PODER)
                {
                    npc.currentMana -= 1;
                    
                    int diff = player.magicPower - npc.magicPower;
                    if (diff > 0)
                    {
                        result.npcDamage = diff;
                        result.description = $"Player PODER vence! Causa {diff}!";
                    }
                    else if (diff < 0)
                    {
                        result.playerDamage = -diff;
                        result.description = $"NPC PODER vence! Toma {-diff}!";
                    }
                    else
                    {
                        result.description = "PODER vs PODER - Empate!";
                    }
                }
                else if (npcCard == CardType.PODER_NEGRO)
                {
                    result.npcDamage = player.magicPower;
                    result.description = $"PODER vence PODER NEGRO! Causa {player.magicPower}!";
                }
                else if (npcCard == CardType.ESCUDO)
                {
                    result.npcDamage = Mathf.CeilToInt(player.magicPower / 2f);
                    result.description = $"PODER causa {result.npcDamage} no ESCUDO!";
                }
            }
            
            // === GRAB ===
            else if (playerCard == CardType.GRAB)
            {
                if (npcCard == CardType.PODER)
                {
                    result.npcDamage = npc.magicPower;
                    result.description = $"GRAB refletiu PODER! NPC toma {npc.magicPower}!";
                }
                else if (npcCard == CardType.ESPADA)
                {
                    result.playerDamage = npc.attackPower;
                    result.description = $"ESPADA vence! Toma {npc.attackPower}!";
                }
                else if (npcCard == CardType.MEDITACAO)
                {
                    player.currentXP += 1;
                    result.description = "GRAB deu +1 XP para MEDITAÇÃO!";
                }
                else if (npcCard == CardType.PODER_NEGRO)
                {
                    result.playerDamage = Mathf.CeilToInt(npc.magicPower / 2f);
                    result.description = $"PODER NEGRO causa {result.playerDamage} no GRAB!";
                }
                else if (npcCard == CardType.ESCUDO) // ← ADICIONADO
                {
                    result.npcDamage = Mathf.CeilToInt(player.attackPower / 2f);
                    result.description = $"GRAB causa {result.npcDamage} no ESCUDO!";
                }
            }
            
            // === MEDITAÇÃO ===
            else if (playerCard == CardType.MEDITACAO)
            {
                player.currentHealth = Mathf.Min(player.health, player.currentHealth + 2);
                
                if (npcCard == CardType.GRAB)
                {
                    npc.currentXP += 1;
                    result.description = "MEDITAÇÃO curou 2 HP!";
                }
                else if (npcCard == CardType.PODER_NEGRO)
                {
                    result.npcDamage = Mathf.CeilToInt(npc.magicPower / 2f);
                    result.description = $"MEDITAÇÃO refletiu PODER NEGRO! NPC toma {result.npcDamage}!";
                }
                else if (npcCard == CardType.ESPADA)
                {
                    result.playerDamage = npc.attackPower;
                    result.description = $"ESPADA vence! Toma {npc.attackPower}! (mas curou 2 antes)";
                }
                else if (npcCard == CardType.PODER)
                {
                    result.playerDamage = npc.magicPower;
                    result.description = $"PODER vence! Toma {npc.magicPower}! (mas curou 2 antes)";
                }
                else if (npcCard == CardType.ESCUDO)
                {
                    result.description = "MEDITAÇÃO curou 2 HP! Empate com ESCUDO.";
                }
            }
            
            // === PODER NEGRO ===
            else if (playerCard == CardType.PODER_NEGRO)
            {
                int damageValue = Mathf.CeilToInt(player.magicPower / 2f);
                
                if (npcCard == CardType.GRAB)
                {
                    result.npcDamage = damageValue;
                    result.description = $"PODER NEGRO causa {damageValue} no GRAB!";
                }
                else if (npcCard == CardType.PODER_NEGRO)
                {
                    result.description = "PODER NEGRO vs PODER NEGRO - Empate!";
                }
                else if (npcCard == CardType.ESPADA)
                {
                    result.playerDamage = npc.attackPower;
                    result.description = $"ESPADA vence! Toma {npc.attackPower}!";
                }
                else if (npcCard == CardType.MEDITACAO)
                {
                    result.playerDamage = damageValue;
                    result.description = $"MEDITAÇÃO refletiu! Toma {damageValue}!";
                }
                else if (npcCard == CardType.PODER)
                {
                    result.playerDamage = npc.magicPower;
                    result.description = $"PODER vence PODER NEGRO! Toma {npc.magicPower}!";
                }
                else if (npcCard == CardType.ESCUDO)
                {
                    result.description = "ESCUDO bloqueou PODER NEGRO!";
                }
            }
            
            return result;
        }
    }
    
    public class CombatResult
    {
        public int playerDamage = 0;
        public int npcDamage = 0;
        public string description = "";
    }
}