using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class VR_InteractableObject : MonoBehaviour
    {
        public Material highlightMaterial;
        [HideInInspector] public new Rigidbody rigidbody;
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

        /// <summary>
        /// Used to add highlight material to all of the renderers
        /// </summary>
        public void ActivateHighlight()
        {
            MeshRenderer[] rendererGO = GetComponentsInChildren<MeshRenderer>();
            foreach (var item in rendererGO)
            {
                Material[] newMaterials = new Material[item.materials.Length + 1];
                for (int i = 0; i < item.materials.Length; i++)
                {
                    newMaterials[i] = item.materials[i];
                }
                newMaterials[newMaterials.Length - 1] = highlightMaterial;
                item.materials = newMaterials;
            }
        }

        /// <summary>
        /// Used to remove highlight material to all of the renderers
        /// </summary>
        public void DesactivateHighlight()
        {
            MeshRenderer[] rendererGO = GetComponentsInChildren<MeshRenderer>();
            foreach (var item in rendererGO)
            {
                Material[] newMaterials = new Material[item.materials.Length - 1];
                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = item.materials[i];
                }
                item.materials = newMaterials;
            }
        }
    }
}
