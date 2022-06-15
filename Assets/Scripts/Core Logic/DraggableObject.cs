using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Core_Logic
{
    public class DraggableObject : MonoBehaviour
    {
        [SerializeField] private float catFoundDistance = 0.8f;
        [SerializeField] private float maxDragDistance = 1f;
        [SerializeField] private float returningTime = 0.5f;
        [SerializeField] private float clickableRadius = 0.7f;
        [SerializeField] private AudioClip soundBeginDrag;
        [SerializeField] private AudioClip soundEndDrag;

        public CatView cat;

        private Vector3 initialPosition;

        private bool isDragging;
        private bool isReturning;

        private MonoBehaviourSingleton main;

        private void Awake()
        {
            initialPosition = transform.position;
            Debug.Log("Initial Pos " + initialPosition);
            main = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
        }

        private void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (isDragging)
            {
               ToDrag(mousePos);
            }
            
            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                main.PlaySound(soundEndDrag);
                SetDragOff();
            }

            if (Input.GetMouseButtonDown(0) 
                && !isDragging && !isReturning && main.State == MonoBehaviourSingleton.PlayerState.Idling
                && Vector3.Distance(mousePos, initialPosition) < clickableRadius)
            {
                main.PlaySound(soundBeginDrag);
                SetDragOn();
            }
        }

        private void ToDrag(Vector3 mousePos)
        {
            Vector3 allowedPos = mousePos - initialPosition;
            allowedPos = Vector3.ClampMagnitude(allowedPos, maxDragDistance);
            transform.position = initialPosition + allowedPos;
        }
        
        public void SetDragOff()
        {
            if (cat != null && Vector3.Distance(transform.position, initialPosition) > catFoundDistance)
            {
                main.OnCatFound(cat);
                cat = null;
            }
            
            main.CurrentDrag = null;
            isDragging = false;
            isReturning = true;
            main.State = main.DefaultState;
            transform.DOMove(initialPosition, returningTime).OnComplete(() =>
            {
                isReturning = false;
            });
        }

        private void SetDragOn()
        {
            isDragging = true;
            main.State = MonoBehaviourSingleton.PlayerState.DraggingItem;
            main.CurrentDrag = this;
        }
    }
}
