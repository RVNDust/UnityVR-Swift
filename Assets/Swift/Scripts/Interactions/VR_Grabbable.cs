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

        void Awake()
        {
            interactable = GetComponent<VR_InteractableObject>();
        }

        /// <summary>
        /// Function to call to grab an object
        /// </summary>
        /// <param name="controller">controller on which the object will be attached</param>
        public void Grab(GameObject controller)
        {
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
                controllerReference = null;

                interactable.SetOriginalState();

                //Throw object
                interactable.rigidbody.velocity = controllerReference.GetComponent<SteamVR_Behaviour_Pose>().GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
                interactable.rigidbody.angularVelocity = controllerReference.GetComponent<SteamVR_Behaviour_Pose>().GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();
            }
            if (onUngrabEvent != null)
            {
                onUngrabEvent();
            }
        }
    }
}

