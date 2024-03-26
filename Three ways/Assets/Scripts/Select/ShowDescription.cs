using Fight;
using Fight.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Select
{
    public class ShowDescription : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler
    {
        public Steel index;
        public GameObject board;

        public void OnPointerDown(PointerEventData eventData)
        {   
            GetComponent<AudioSource>().Play();
            transform.localScale = 1.1f * transform.localScale;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale /= 1.1f;
            board.SetActive(true);
            board.GetComponent<DescriptionBoard>().SetData(index);
        }
    }
}
