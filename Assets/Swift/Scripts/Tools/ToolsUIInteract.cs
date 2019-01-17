//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Valve.VR;

namespace Swift
{
    public class ToolsUIInteract : ToolBehaviour
    {
        public LayerMask UILayerMask;
        public float interactLimitDistance = 5.0f;
        public GameObject markerPrefab;

        public Transform raycastOrigin;
        private GameObject markerInstance;

        void Start()
        {
            raycastOrigin = transform.Find("UIRaycast");
            markerInstance = Instantiate(markerPrefab);
            markerInstance.SetActive(false);
        }

        void Update()
        {
            if(raycastOrigin != null)
            {
                Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, interactLimitDistance, UILayerMask))
                {
                    markerInstance.SetActive(true);
                    markerInstance.transform.position = hit.transform.position;
                    //Check if the left Mouse button is clicked
                    if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(controller))
                    {
                        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                        if(hit.collider.GetComponent<IPointerClickHandler>() != null)
                            hit.collider.GetComponent<IPointerClickHandler>().OnPointerClick(pointerEventData);
                    }
                }
            }
        }
    }
}