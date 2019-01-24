//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Valve.VR;
using Swift.UI;

namespace Swift.Tools
{
    public class ToolsUIInteract : ToolBehaviour
    {
        public SteamVR_Action_Vibration haptics;

        public LayerMask UILayerMask;
        public float interactLimitDistance = 5.0f;
        public GameObject markerPrefab;

        public Transform raycastOrigin;
        private GameObject markerInstance;

        void Start()
        {
            raycastOrigin = transform.Find("UIRaycast");
            markerInstance = Instantiate(markerPrefab);
        }

        void Update()
        {
            if(raycastOrigin != null)
            {
                Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, interactLimitDistance, UILayerMask))
                {
                    CanvasBehaviour window = hit.transform.GetComponent<CanvasBehaviour>();
                    if(window != null)
                    {
                        markerInstance.SetActive(true);
                        markerInstance.transform.parent = window.transform;
                        markerInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        MarkerPositioning(hit);
                    }
                    MarkerPositioning(hit);

                    if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(controller))
                    {
                        haptics.Execute(0, .05f, 50, .25f, controller);
                        MarkerEventData pointerEventData = new MarkerEventData(EventSystem.current, hit.transform);
                        if (hit.collider.GetComponent<IPointerClickHandler>() != null)
                        {
                            hit.collider.GetComponent<IPointerClickHandler>().OnPointerClick(pointerEventData);
                        }
                        if (hit.collider.GetComponent<IPointerDownHandler>() != null)
                        {
                            hit.collider.GetComponent<IPointerDownHandler>().OnPointerDown(pointerEventData);
                        }
                    }

                    //if (SteamVR_Input._default.inActions.InteractUI.GetStateUp(controller))
                    //{
                    //    MarkerEventData pointerEventData = new MarkerEventData(EventSystem.current, hit.transform);
                    //    if (hit.collider.GetComponent<IPointerUpHandler>() != null)
                    //    {
                    //        hit.collider.GetComponent<IPointerUpHandler>().OnPointerUp(pointerEventData);
                    //    }
                    //}
                }
                else
                {
                    markerInstance.SetActive(false);
                }
            }
            else
            {
                markerInstance.SetActive(false);
            }
        }

        void MarkerPositioning(RaycastHit hit)
        {
            markerInstance.transform.position = hit.point;
        }
    }

    public class MarkerEventData : PointerEventData
    {
        public Transform Marker;

        public MarkerEventData(EventSystem eventSystem, Transform marker) : base(eventSystem)
        {
            Marker = marker;
        }
    }
}