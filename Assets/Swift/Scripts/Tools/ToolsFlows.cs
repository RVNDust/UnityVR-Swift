using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class ToolsFlows : ToolBehaviour
    {
        public GameObject FlowWindowPrefab;
        public GameObject FlowInformationPrefab;

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
            FlowCanvasBehaviour window = vrPlayer.GetComponent<WindowsManager>().CreateWindow(FlowWindowPrefab).GetComponent<FlowCanvasBehaviour>(); //Délègue la création de la fenêtre
            goRef.AddComponent<ToolsFlows>(); //Ajout du comportement spécifique sur le controller
            FlowManager.Instance.ToggleDisplayFlowPath(true); //Active l'affichage des flux
            if(!FlowManager.Instance.IsFilled)
            { 
                foreach (var item in FlowManager.Instance.productFlows)
                {
                    FlowInformationsBehaviour informations = window.GetComponent<FlowCanvasBehaviour>().CreateProductInformations(FlowInformationPrefab).GetComponent<FlowInformationsBehaviour>();
                    informations.ProductValue = item.Key;
                    informations.DistanceValue = "100";
                    informations.VolumeValue = "5";
                }
                FlowManager.Instance.IsFilled = true;
            }
        }

        public override void DesactivateTool(GameObject goRef)
        {
            vrPlayer.GetComponent<WindowsManager>().ToggleWindowState(FlowWindowPrefab, false);
            FlowManager.Instance.ToggleDisplayFlowPath(false);
            Destroy(goRef.GetComponent<ToolsFlows>());
        }
    }
}
