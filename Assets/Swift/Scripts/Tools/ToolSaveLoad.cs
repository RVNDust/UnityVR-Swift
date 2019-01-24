using Swift.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift.Tools
{
    public class ToolSaveLoad : ToolBehaviour {

        public GameObject SaveLoadWindowPrefab;

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
            SaveLoadCanvasBehaviour window = vrPlayer.GetComponent<WindowsManager>().CreateWindow(SaveLoadWindowPrefab).GetComponent<SaveLoadCanvasBehaviour>(); //Délègue la création de la fenêtre
            goRef.AddComponent<ToolSaveLoad>(); //Ajout du comportement spécifique sur le controller
        }

        public override void DesactivateTool(GameObject goRef)
        {
            vrPlayer.GetComponent<WindowsManager>().ToggleWindowState(SaveLoadWindowPrefab, false);
            Destroy(goRef.GetComponent<ToolSaveLoad>());
        }
    }
}
