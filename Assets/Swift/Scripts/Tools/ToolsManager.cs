using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift
{
    public class ToolsManager : MonoBehaviour
    {
        public SteamVR_Input_Sources controller;

        // Use this for initialization
        void Start()
        {
            controller = gameObject.GetComponent<SteamVR_Behaviour_Pose>().inputSource;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
