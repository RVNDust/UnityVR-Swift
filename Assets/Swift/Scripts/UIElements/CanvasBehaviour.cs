using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift
{
    public class CanvasBehaviour : MonoBehaviour, IPointerClickHandler
    {
        Canvas window;
        BoxCollider boxCollider;

        void Awake()
        {
            AwakeBehaviour();
        }

        protected void AwakeBehaviour()
        {
            window = gameObject.GetComponent<Canvas>();
            boxCollider = gameObject.GetComponent<BoxCollider>();
            if (boxCollider == null)
                boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        void Start()
        {
            StartBehaviour();
        }

        protected void StartBehaviour()
        {
            boxCollider.isTrigger = true;
            Vector3 boxSize = new Vector3(window.GetComponent<RectTransform>().rect.width, window.GetComponent<RectTransform>().rect.height, 1);
            boxCollider.size = boxSize;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}
