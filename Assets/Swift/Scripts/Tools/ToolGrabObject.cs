using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Swift
{
    public class ToolGrabObject : ToolBehaviour
    {
        List<GameObject> heldObjects =  new List<GameObject>();

        void OnTriggerStay(Collider collider)
        {
            VR_Grabbable grabbableObject = collider.GetComponent<VR_Grabbable>();
            if(grabbableObject != null)
            {
                //Check if button for grab is used, if true grab an object, if not ungrab it
                if(SteamVR_Input._default.inActions.GrabGrip.GetState(controller))
                {
                    grabbableObject.Grab(gameObject);
                    heldObjects.Add(grabbableObject.gameObject);
                }
                else
                {
                    grabbableObject.Ungrab(gameObject);
                    heldObjects.Remove(grabbableObject.gameObject);
                }
            }
        }
    }
}
