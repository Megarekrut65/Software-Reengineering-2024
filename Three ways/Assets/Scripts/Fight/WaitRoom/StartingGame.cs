using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.WaitRoom
{
    public class StartingGame : MonoBehaviour
    {
        public GameObject gameManager;
        public Text count;
        public void Game()
        {
            StartCoroutine(Count());
        }

        private IEnumerator Count()
        {
            for(int i = 3; i >= 0; i--)
            {
                count.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            gameManager.GetComponent<GameManager.GameManager>().StartGame();
            gameObject.SetActive(false); 
            StopCoroutine(Count());
        }
    }
}
