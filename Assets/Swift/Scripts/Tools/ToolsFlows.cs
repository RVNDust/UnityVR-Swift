using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class ToolsFlows : ToolBehaviour
    {
        public GameObject flowManager;

        void Awake()
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
            goRef.AddComponent<ToolsFlows>();
            flowManager.GetComponent<FlowManager>().ToggleDisplayFlowPath(true);
        }

        public override void DesactivateTool(GameObject goRef)
        {
            flowManager.GetComponent<FlowManager>().ToggleDisplayFlowPath(false);
            Destroy(goRef.GetComponent<ToolsFlows>());
        }
    }
}
