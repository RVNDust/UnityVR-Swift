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
            //SaveLoadCanvasBehaviour window = vrPlayer.GetComponent<WindowsManager>().CreateWindow(SaveLoadWindowPrefab).GetComponent<SaveLoadCanvasBehaviour>(); //Délègue la création de la fenêtre
            //goRef.AddComponent<ToolScreenshot>().screenshotManager = cam.GetComponent<ScreenshotManager>(); //Ajout du comportement spécifique sur le controller
        }

        public override void DesactivateTool(GameObject goRef)
        {
            Destroy(goRef.GetComponent<ToolScreenshot>());
        }
    }
}

