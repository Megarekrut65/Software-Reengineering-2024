using UnityEngine;
using UnityEngine.UI;

namespace Fight.WaitRoom
{
    public class SelectBackground : MonoBehaviour
    {
        public Image[] backgrounds;
        void Start()
        {
            int size = backgrounds.Length;
            int index = Random.Range(0, size);
            GetComponent<Image>().sprite = backgrounds[index].sprite;
        }
    }
}
