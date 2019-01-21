using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift
{
    public class ToolScreenshot : ToolBehaviour
    {
        public ScreenshotManager screenshotManager;

        void Start()
        {
            StartBehaviour();
        }

        void Update()
        {
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateUp(controller))
            {
                Camera cam = vrPlayer.GetComponentInChildren<Camera>();
                screenshotManager.TakeScreenshot(cam);
            }
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
            GameObject cam = vrPlayer.GetComponentInChildren<Camera>().gameObject;
            if(cam.GetComponent<ScreenshotManager>() == null)
            {
                cam.AddComponent<ScreenshotManager>();
            }
            goRef.AddComponent<ToolScreenshot>().screenshotManager = cam.GetComponent<ScreenshotManager>(); //Ajout du comportement spécifique sur le controller
        }

        public override void DesactivateTool(GameObject goRef)
        {
            Destroy(goRef.GetComponent<ToolScreenshot>());
        }
    }
}

