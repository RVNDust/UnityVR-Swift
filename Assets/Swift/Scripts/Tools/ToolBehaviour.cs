using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

namespace Swift
{
    [RequireComponent(typeof(ToolsManager))]
    public class ToolBehaviour : MonoBehaviour
    {
        protected SteamVR_Input_Sources controller;
        protected GameObject vrPlayer;
        protected ToolsManager manager;
        protected List<GameObject> collidedObjects = new List<GameObject>();

        // Use this for initialization
        void Awake()
        {
            StartBehaviour();
        }

        /// <summary>
        /// Base to call in start for children
        /// </summary>
        protected void StartBehaviour()
        {
            manager = GetComponent<ToolsManager>();
            if(GetComponent<SteamVR_Behaviour_Pose>() != null)
                controller = GetComponent<SteamVR_Behaviour_Pose>().inputSource;
            GetPlayerReference();
        }

        protected void GetPlayerReference()
        {
            GameObject[] playersEntities = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playersEntities)
            {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    vrPlayer = player;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            TriggerEnterBehaviour(other);
        }

        /// <summary>
        /// Base to call in trigger enter for children
        /// </summary>
        /// <param name="other"></param>
        protected void TriggerEnterBehaviour(Collider other)
        {
            GameObject container = other.GetComponentInParent<Rigidbody>().gameObject;
            if (!collidedObjects.Contains(container) && container != null)
            {
                collidedObjects.Add(container);
                VR_InteractableObject io = container.GetComponent<VR_InteractableObject>();
                if(io != null)
                {
                    io.ActivateHighlight();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExitBehaviour(other);
        }

        /// <summary>
        /// Base to call in trigger exit for children
        /// </summary>
        /// <param name="other"></param>
        protected void TriggerExitBehaviour(Collider other)
        {
            Rigidbody containerRb = other.GetComponentInParent<Rigidbody>();
            if (containerRb != null && collidedObjects.Contains(containerRb.gameObject))
            {
                GameObject container = containerRb.gameObject;
                collidedObjects.Remove(container);
                VR_InteractableObject io = container.GetComponent<VR_InteractableObject>();
                if (io != null)
                {
                    io.DesactivateHighlight();
                }
            }
        }

        /// <summary>
        /// Redéfinir dans les enfants pour déterminer le comportement d'un outil à l'activation
        /// </summary>
        /// <param name="goRef">Controller sur le lequel est ajouté l'outil</param>
        public virtual void ActivateTool(GameObject goRef)
        {
            goRef.AddComponent <ToolBehaviour> ();
        }

        /// <summary>
        /// Redéfinir dans les enfants pour déterminer le comportement d'un outil à l'désactivation
        /// </summary>
        /// <param name="goRef">Controller sur duquel est retiré l'outil</param>
        public virtual void DesactivateTool(GameObject goRef)
        {
            Destroy(goRef.GetComponent<ToolBehaviour>());
        }
        
    }
}
