using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift
{
    [RequireComponent(typeof(ToolsManager))]
    public class ToolBehaviour : MonoBehaviour
    {
        protected SteamVR_Input_Sources controller;
        protected ToolsManager manager;
        protected List<GameObject> collidedObjects = new List<GameObject>();
        public List<string> ignoredLayers = new List<string>();

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
            controller = GetComponent<SteamVR_Behaviour_Pose>().inputSource;

            ignoredLayers.Add("Ignore Raycast");
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
            if (!collidedObjects.Contains(other.gameObject))
            {
                collidedObjects.Add(other.gameObject);
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
            if (collidedObjects.Contains(other.gameObject))
            {
                collidedObjects.Remove(other.gameObject);
            }
        }
    }
}
