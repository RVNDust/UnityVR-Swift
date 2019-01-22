using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift
{
    public class ToolRadar : ToolBehaviour {

        public GameObject RadarWindowPrefab;

        void Start()
        {
            StartBehaviour();
        }

        void OnTriggerEnter(Collider other)
        {
            TriggerEnterBehaviour(other);
        }

        void OnTriggerExit(Collider other)
        {
            TriggerExitBehaviour(other);
        }

        public override void ActivateTool(GameObject goRef)
        {
            StartBehaviour();
            RadarCanvasBehaviour window = vrPlayer.GetComponent<WindowsManager>().CreateWindow(RadarWindowPrefab).GetComponent<RadarCanvasBehaviour>(); //Délègue la création de la fenêtre
        }

        public override void DesactivateTool(GameObject goRef)
        {
            vrPlayer.GetComponent<WindowsManager>().ToggleWindowState(RadarWindowPrefab, false);
        }
    }
}

