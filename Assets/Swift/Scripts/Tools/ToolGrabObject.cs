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
        GameObject vrPlayer;

        void Awake()
        {
            StartBehaviour();
        }

        void Start()
        {
            GameObject[] playersEntities = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playersEntities)
            {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    vrPlayer = player;
                }
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
                    heldObjects.Remove(grabbableObject.gameObject);
                    vrPlayer.GetComponent<VR_CameraRigMultiuser>().CmdLoseControl(item);
                }
            }
        }

        //void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log("Trigger stay");
        //    VR_Grabbable grabbableObject = other.gameObject.GetComponent<VR_Grabbable>();
        //    if(grabbableObject != null)
        //    {
        //        Debug.Log("Inside a grabbable object");
        //        //Check if button for grab is used, if true grab an object, if not ungrab it
        //        if(SteamVR_Input._default.inActions.GrabGrip.GetState(controller))
        //        {
        //            Debug.Log("Trying to grab something");
        //            grabbableObject.Grab(gameObject);
        //            heldObjects.Add(grabbableObject.gameObject);
        //        }
        //        else
        //        {
        //            grabbableObject.Ungrab(gameObject);
        //            heldObjects.Remove(grabbableObject.gameObject);
        //        }
        //    }
        //}
    }
}
