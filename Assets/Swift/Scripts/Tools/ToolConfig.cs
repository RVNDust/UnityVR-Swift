using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class ToolConfig : ToolBehaviour
    {

        public GameObject TargetWindowPrefab;

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
            TargetStateCanvasBehaviour window = vrPlayer.GetComponent<WindowsManager>().CreateWindow(TargetWindowPrefab).GetComponent<TargetStateCanvasBehaviour>(); //Délègue la création de la fenêtre
            goRef.AddComponent<ToolConfig>(); //Ajout du comportement spécifique sur le controller
        }

        public override void DesactivateTool(GameObject goRef)
        {
            vrPlayer.GetComponent<WindowsManager>().ToggleWindowState(TargetWindowPrefab, false);
            Destroy(goRef.GetComponent<ToolConfig>());
        }
    }
}
