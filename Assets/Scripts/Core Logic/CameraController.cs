using UnityEngine;
using UnityEngine.EventSystems;

namespace Core_Logic
{
    public class CameraController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField] private float cameraSmoothing = 0.05f;
        [SerializeField] private float cameraSensivity = 1f;
        
        private Vector3 cameraTarget;
        private MonoBehaviourSingleton mainSingleton;

        private bool isScrolling;
        
        private void Awake()
        {
            mainSingleton = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
        }

        private void Update()
        {
            Vector3 newPos = Vector3.Lerp(Camera.main.transform.position, cameraTarget, cameraSmoothing);
            newPos.z = -10;
            Camera.main.transform.position = newPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isScrolling)
            {
                Camera mainCamera = Camera.main;
                
                if (mainCamera != null)
                {
                    Vector3 cam = mainCamera.transform.position;
                    float width = mainCamera.orthographicSize * 2.0f * Screen.width / Screen.height;
                    Bounds bounds = mainSingleton.Renderer.bounds;
                    cameraTarget =
                        new Vector3(
                            Mathf.Clamp(cam.x + eventData.delta.x * 0.1f * cameraSensivity, bounds.min.x + width / 2,
                                bounds.max.x - width / 2), 0, -10);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (mainSingleton.State == MonoBehaviourSingleton.PlayerState.Scrolling && isScrolling)
            {
                mainSingleton.State = MonoBehaviourSingleton.PlayerState.Idling;
                isScrolling = false;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (mainSingleton.State == MonoBehaviourSingleton.PlayerState.Idling && !isScrolling)
            {
                mainSingleton.State = MonoBehaviourSingleton.PlayerState.Scrolling;
                isScrolling = true;
            }
        }
    }
}
