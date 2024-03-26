using UnityEngine;

namespace Photon.PhotonChat.Demos.DemoChat
{
    public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return gameObject.activeInHierarchy;
        }
    }
}