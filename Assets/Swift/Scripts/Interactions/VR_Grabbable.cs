using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift
{
    [RequireComponent(typeof(VR_InteractableObject))]
    public class VR_Grabbable : MonoBehaviour
    {
        #region Events
        public delegate void OnGrabEvent();
        public static event OnGrabEvent onGrabEvent;

        public delegate void OnUngrabEvent();
        public static event OnUngrabEvent onUngrabEvent;
        #endregion

        private VR_InteractableObject interactable;
        private GameObject controllerReference;
        private Vector3 shrinkVelocity;
        private Vector3 originalScale;
        private Vector3 shrinkScale;
        [Range(1, 10)] public float shrinkFactor = 10;
        public bool IsGrabbed;

        void Awake()
        {
            interactable = GetComponent<VR_InteractableObject>();
        }

        void Start()
        {
            originalScale = gameObject.transform.localScale;
            shrinkScale = originalScale / shrinkFactor;
        }

        void Update()
        {
            if(IsGrabbed)
            {
                Debug.Log("Shrink");
                gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, shrinkScale, ref shrinkVelocity, 0.5f);
            }
            else
            {
                gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, originalScale, ref shrinkVelocity, 0.5f);
            }
        }

        /// <summary>
        /// Function to call to grab an object
        /// </summary>
        /// <param name="controller">controller on which the object will be attached</param>
        public void Grab(GameObject controller)
        {
            Debug.Log("Call grab method");
            interactable.GetOriginalState();

            interactable.rigidbody.isKinematic = true;
            transform.SetParent(controller.gameObject.transform);

            controllerReference = controller;

            if(onGrabEvent != null)
            {
                onGrabEvent();
            }
        }

        /// <summary>
        /// Function to call to ungrab an object
        /// </summary>
        /// <param name="controller">Controller on which the object tries to be detached</param>
        public void Ungrab(GameObject controller)
        {
            if(controllerReference == controller)
            {
                interactable.SetOriginalState();

                //Throw object
                interactable.rigidbody.velocity = controllerReference.GetComponent<SteamVR_Behaviour_Pose>().GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
                interactable.rigidbody.angularVelocity = controllerReference.GetComponent<SteamVR_Behaviour_Pose>().GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();

                controllerReference = null;
            }
            if (onUngrabEvent != null)
            {
                onUngrabEvent();
            }
        }
    }
}

