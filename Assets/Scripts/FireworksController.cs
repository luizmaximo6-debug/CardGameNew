using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace SinuousProductions
{
    public class FireworksController : MonoBehaviour
    {
        public static FireworksController Instance;

        public List<Animator> fireworks;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
            foreach (var fw in fireworks)
                if (fw != null) fw.gameObject.SetActive(false);
        }

        public void PlayFireworks()
        {
            StartCoroutine(FireworksSequence());
        }

        IEnumerator FireworksSequence()
        {
            foreach (var fw in fireworks)
            {
                if (fw == null) continue;

                float clipLength = fw.runtimeAnimatorController.animationClips[0].length;

                fw.gameObject.SetActive(true);
                fw.Rebind();
                fw.Play(0);

                yield return new WaitForSeconds(clipLength);

                fw.gameObject.SetActive(false);
            }

            // Reseta o título para aparecer de novo
            PlayerPrefs.DeleteKey("TituloMostrado");
            PlayerPrefs.Save();

            SceneManager.LoadScene(0);
        }
    }
}