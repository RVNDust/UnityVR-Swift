using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift.Interactions
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

        public GameObject boundingBoxPrefab;
        private GameObject boundingBox;

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
                gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, shrinkScale, ref shrinkVelocity, 0.5f);

                if(boundingBoxPrefab != null && boundingBox == null)
                {
                    boundingBox = Instantiate(boundingBoxPrefab);
                    Vector3 meshSize = CalculateLocalBounds().extents;
                    Vector3 boxSize = new Vector3(meshSize.x, 1, meshSize.z);
                    boundingBox.transform.localScale = boxSize;
                }

                if(boundingBox != null)
                {
                    LayerMask layerMask = LayerMask.GetMask("Ground");
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
                    {
                        boundingBox.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                        boundingBox.transform.rotation = Quaternion.Euler(boundingBox.transform.rotation.eulerAngles.x,
                                                                          gameObject.transform.rotation.eulerAngles.y,
                                                                          boundingBox.transform.rotation.eulerAngles.z);
                    }
                }
            }
            else
            {
                gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, originalScale, ref shrinkVelocity, 0.5f);

                if (boundingBox != null)
                    Destroy(boundingBox);
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
            Collider[] collidersCollection = gameObject.GetComponentsInChildren<Collider>();
            foreach (var item in collidersCollection)
            {
                item.isTrigger = false;
            }
            transform.SetParent(controller.gameObject.transform);
            interactable.originalLocalPosition = interactable.transform.localPosition;

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

        /// <summary>
        /// Method to calculate localBounds of an object (found on internet)
        /// </summary>
        /// <returns></returns>
        Bounds CalculateLocalBounds()
        {
            Quaternion currentRotation = this.transform.rotation;
            this.transform.rotation = Quaternion.Euler(0f,0f,0f);
            Bounds bounds = new Bounds(this.transform.position, Vector3.zero);
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(r.bounds);
            }
            Debug.Log(bounds);
            return bounds;
        }
    }
}

