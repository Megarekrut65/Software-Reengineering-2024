using UnityEngine;

namespace Main
{
    public class MyOpenURL : MonoBehaviour
    {
        public string url = "http://";
        public void Open()
        {
            Application.OpenURL(url);
        }
    }
}
