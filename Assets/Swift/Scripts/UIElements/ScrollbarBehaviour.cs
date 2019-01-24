using Swift.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift.UI
{
    public class ScrollbarBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        protected Scrollbar scrollbar;
        protected BoxCollider boxCollider;
        protected Transform markerRef;
        Vector3 lastPosition;

        void Awake()
        {
            AwakeBehaviour();
        }

        protected void AwakeBehaviour()
        {
            scrollbar = GetComponent<Scrollbar>();
            boxCollider = GetComponent<BoxCollider>();
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

            //Vector3 boxSize = new Vector3(btn.GetComponent<RectTransform>().sizeDelta.x , btn.GetComponent<RectTransform>().rect.height, -1);
            //Debug.Log(boxSize);
            boxCollider.size = new Vector3(100, 50, 1);
            //boxCollider.size = boxSize;
            boxCollider.center = new Vector3(0, 0, -1);
        }

        void Update()
        {
            if(markerRef != null)
            {
                if(lastPosition.y > markerRef.transform.position.y)
                {
                    scrollbar.value += 0.01f;
                }

                if (lastPosition.y < markerRef.transform.position.y)
                {
                    scrollbar.value -= 0.01f;
                }
                scrollbar.value = Mathf.Clamp(scrollbar.value, 0, 1);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MarkerEventData e = eventData as MarkerEventData;
            markerRef = e.Marker;
            lastPosition = markerRef.transform.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            lastPosition = Vector3.zero;
            markerRef = null;
        }
    }
}

