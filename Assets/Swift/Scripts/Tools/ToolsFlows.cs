using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class ToolsFlows : ToolBehaviour
    {
        public GameObject FlowManager;
        public GameObject FlowWindowPrefab;
        public GameObject FlowInformationPrefab;

        void Start()
        {
            StartBehaviour();
            FlowManager = transform.Find("/FlowManager").gameObject;
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
            FlowManager = transform.Find("/FlowManager").gameObject;
            FlowCanvasBehaviour window = vrPlayer.GetComponent<WindowsManager>().CreateWindow(FlowWindowPrefab).GetComponent<FlowCanvasBehaviour>(); //Délègue la création de la fenêtre
            goRef.AddComponent<ToolsFlows>(); //Ajout du comportement spécifique sur le controller
            FlowManager.GetComponent<FlowManager>().ToggleDisplayFlowPath(true); //Active l'affichage des flux
            foreach (var item in FlowManager.GetComponent<FlowManager>().productFlows)
            {
                window.GetComponent<FlowCanvasBehaviour>().CreateProductInformations(FlowInformationPrefab);
            }
        }

        public override void DesactivateTool(GameObject goRef)
        {
            vrPlayer.GetComponent<WindowsManager>().ToggleWindowState(FlowWindowPrefab, false);
            FlowManager.GetComponent<FlowManager>().ToggleDisplayFlowPath(false);
            Destroy(goRef.GetComponent<ToolsFlows>());
        }
    }
}
