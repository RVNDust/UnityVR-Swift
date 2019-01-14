using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift
{
    [RequireComponent(typeof(ToolsManager))]
    public class ToolBehaviour : MonoBehaviour
    {

        protected ToolsManager manager;
        protected SteamVR_Input_Sources controller;

        // Use this for initialization
        void Awake()
        {
            manager = GetComponent<ToolsManager>();
            controller = manager.controller;
        }
    }
}
