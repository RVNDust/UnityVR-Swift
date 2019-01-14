using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class VR_InteractableObject : MonoBehaviour
    {

        [HideInInspector] public Rigidbody rigidbody;
        [HideInInspector] public bool originalKinematicState;
        [HideInInspector] public Transform originalParent;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            GetOriginalState();
        }

        /// <summary>
        /// Register actual parent of the gameobject and kinematic state
        /// </summary>
        public void GetOriginalState()
        {
            originalParent = transform.parent;
            originalKinematicState = rigidbody.isKinematic;
        }

        /// <summary>
        /// Return original parent of the gameobject and kinematic state
        /// </summary>
        public void SetOriginalState()
        {
            transform.parent = originalParent;
            rigidbody.isKinematic = originalKinematicState;
        }
    }
}
