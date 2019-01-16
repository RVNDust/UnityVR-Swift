using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class ToolsFlows : ToolBehaviour
    {
        public GameObject flowManager;
        public GameObject flowWindowPrefab;

        void Start()
        {
            StartBehaviour();
            flowManager = transform.Find("/FlowManager").gameObject;
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
            flowManager = transform.Find("/FlowManager").gameObject;
            vrPlayer.GetComponent<WindowsManager>().CreateWindow(flowWindowPrefab); //Délègue la création de la fenêtre
            goRef.AddComponent<ToolsFlows>(); //Ajout du comportement spécifique sur le controller
            flowManager.GetComponent<FlowManager>().ToggleDisplayFlowPath(true); //Active l'affichage des flux

        }

        public override void DesactivateTool(GameObject goRef)
        {
            vrPlayer.GetComponent<WindowsManager>().ToggleWindowState(flowWindowPrefab, false);
            flowManager.GetComponent<FlowManager>().ToggleDisplayFlowPath(false);
            Destroy(goRef.GetComponent<ToolsFlows>());
        }
    }
}
