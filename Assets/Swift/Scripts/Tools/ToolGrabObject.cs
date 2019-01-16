using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

namespace Swift
{
    public class ToolGrabObject : ToolBehaviour
    {
        List<GameObject> heldObjects =  new List<GameObject>();

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

        void Update()
        {
            if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(controller))
            {
                Debug.Log("Trying to grab something");
                foreach (var item in collidedObjects)
                {
                    if(item != null)
                    {
                        VR_Grabbable grabbableObject = item.GetComponent<VR_Grabbable>();
                        if (grabbableObject != null)
                        {
                            vrPlayer.GetComponent<VR_CameraRigMultiuser>().CmdTakeControl(grabbableObject.gameObject);
                            grabbableObject.Grab(gameObject);
                            grabbableObject.IsGrabbed = true;
                            heldObjects.Add(grabbableObject.gameObject);
                            break;
                        }
                    }
                }
            }
            else if (SteamVR_Input._default.inActions.GrabGrip.GetStateUp(controller))
            {
                foreach (var item in heldObjects)
                {
                    VR_Grabbable grabbableObject = item.GetComponent<VR_Grabbable>();
                    grabbableObject.Ungrab(gameObject);
                    grabbableObject.IsGrabbed = false;
                    vrPlayer.GetComponent<VR_CameraRigMultiuser>().CmdLoseControl(item);
                }
                heldObjects.Clear();
            }
        }

        public override void ActivateTool(GameObject goRef)
        {
            goRef.AddComponent<ToolGrabObject>();
        }

        public override void DesactivateTool(GameObject goRef)
        {
            Destroy(goRef.GetComponent<ToolGrabObject>());
        }
    }
}
