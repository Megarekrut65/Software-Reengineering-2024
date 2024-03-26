using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Registration
{
    public class RegErrors : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler
    {
        public Text errorText;

        public void OnPointerDown(PointerEventData eventData)
        {   
            transform.localScale = 1.1f * transform.localScale;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = transform.localScale / 1.1f;
            CloseError();
        }
        void CloseError()
        {
            gameObject.SetActive(false);
        }
        public void SetError(string message)
        {
            errorText.text = message;
            StartCoroutine("CloseBoard");
        }
        IEnumerator CloseBoard()
        {
            yield return new WaitForSeconds(10f);
            CloseError();
            StopCoroutine("CloseBoard");
        }
    }
}
