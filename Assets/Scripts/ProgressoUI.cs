using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace SinuousProductions
{
    public class ProgressoUI : MonoBehaviour
    {
        public TextMeshProUGUI progressoText;
        public int totalNiveis = 3;

        void Start()
        {
            int nivelAtual = SceneManager.GetActiveScene().buildIndex + 1;
            AtualizarTexto(nivelAtual);
        }

        void AtualizarTexto(int nivel)
        {
            if (progressoText != null)
                progressoText.text = $"{nivel}/{totalNiveis}";
        }

        public static void RegistrarProgresso(int nivel)
        {
            // Não precisa mais de PlayerPrefs
        }
    }
}