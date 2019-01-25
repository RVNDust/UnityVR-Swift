using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift.Tools
{
    public class ToolScreenshot : ToolBehaviour
    {
        public ScreenshotManager screenshotManager;
        ToolsManager tm;

        void Start()
        {
            StartBehaviour();
        }

        void Update()
        {
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateUp(controller))
            {
                Camera cam = vrPlayer.GetComponentInChildren<Camera>();
                tm.haptics.Execute(0, 0.05f, 50, 0.3f, controller);
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

