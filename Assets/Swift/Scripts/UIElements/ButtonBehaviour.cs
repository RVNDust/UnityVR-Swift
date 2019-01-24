using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift
{
    public class ButtonBehaviour : MonoBehaviour
    {
        protected Button btn;
        protected BoxCollider boxCollider;

        void Awake()
        {
            AwakeBehaviour();
        }

        protected void AwakeBehaviour()
        {
            btn = GetComponent<Button>();
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

            btn.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        { 
            //TODO Déclencher changement d'état du bouton
        }
    }
}
