using UnityEngine;

namespace SinuousProductions
{
    public class ManaVisualController : MonoBehaviour
    {
        public static ManaVisualController Instance;

        [Header("Mana Animations")]
        public GameObject playerMana;
        public GameObject npcMana;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
            HideAll();
        }

        public void HideAll()
        {
            if (playerMana != null) playerMana.SetActive(false);
            if (npcMana != null) npcMana.SetActive(false);
        }

        public void AtualizarManaVisual(CardType playerCard, CardType npcCard)
        {
            // Player ganhou mana com Escudo (exceto vs Eye/Meditação)
            if (playerCard == CardType.ESCUDO && npcCard != CardType.MEDITACAO)
                playerMana?.SetActive(true);

            // NPC ganhou mana com Escudo (exceto vs Eye/Meditação)
            if (npcCard == CardType.ESCUDO && playerCard != CardType.MEDITACAO)
                npcMana?.SetActive(true);

            // Player usou PODER — some mana
            if (playerCard == CardType.PODER)
                playerMana?.SetActive(false);

            // NPC usou PODER — some mana
            if (npcCard == CardType.PODER)
                npcMana?.SetActive(false);

            // Eye/Meditação do NPC corta mana do player
            if (npcCard == CardType.MEDITACAO)
                playerMana?.SetActive(false);

            // Eye/Meditação do Player corta mana do NPC
            if (playerCard == CardType.MEDITACAO)
                npcMana?.SetActive(false);
        }
    }
}