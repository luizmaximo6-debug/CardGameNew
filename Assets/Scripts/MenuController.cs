using UnityEngine;
using UnityEngine.SceneManagement;

namespace SinuousProductions
{
    public class MenuController : MonoBehaviour
    {
        public GameObject homeButton;
        public GameObject desligarButton;

        private bool menuAberto = false;

        void Start()
        {
            homeButton.SetActive(false);
            desligarButton.SetActive(false);
        }

        public void OnMenuClicked()
        {
            menuAberto = !menuAberto;
            homeButton.SetActive(menuAberto);
            desligarButton.SetActive(menuAberto);
        }

        public void OnHomeClicked()
        {
            SceneManager.LoadScene(0);
        }

        public void OnDesligarClicked()
        {
            Application.Quit();
        }
    }
}