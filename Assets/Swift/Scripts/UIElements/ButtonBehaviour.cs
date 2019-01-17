using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift
{
    [RequireComponent(typeof(BoxCollider))]
    public class ButtonBehaviour : MonoBehaviour, IPointerClickHandler
    {
        Button btn;
        BoxCollider boxCollider;

        void Awake()
        {
            btn = GetComponentInChildren<Button>();
            boxCollider = GetComponent<BoxCollider>();
        }

        void Start()
        {
            boxCollider.isTrigger = true;
            //Vector3 boxSize = new Vector3(gameObject.transform.rect.size.x, gameObject.transform.rect.size.y, 0);
            boxCollider.size = new Vector3(75, 50, 0);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("I'm here");
        }
    }
}
