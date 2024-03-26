﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LogIn
{
    public class LogInErrors : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler
    {
        public Text errorText;
        public GameObject mainCamera;

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
            mainCamera.GetComponent<LogInAccount>().StopDieAvatars();
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
