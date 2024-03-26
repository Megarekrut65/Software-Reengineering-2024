using Fight.EventHandler;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fight
{
    public class SelectedButton : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler
    {
        public Direction index;
        public GameObject controler;
        public void OnPointerDown(PointerEventData eventData)
        {   
            transform.localScale = 1.1f * transform.localScale;
            controler.GetComponent<AudioSource>().Play();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = transform.localScale / 1.1f;
            controler.GetComponent<SelectedWay>().Select(index);
        }
    }
}
