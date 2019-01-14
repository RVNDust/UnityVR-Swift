using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

namespace Swift
{
	public class ToolTeleport : ToolBehaviour {
        ControllerPointer cp;
        GameObject cameraRig;

        void Start()
        {
            GameObject[] playersEntities = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playersEntities)
            {
                if(player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    cameraRig = player;
                }
            }
        }
		
		void Update () {
			if(SteamVR_Input._default.inActions.Teleport.GetStateDown(controller))
			{
                TeleportPressed();
			}
			else if(SteamVR_Input._default.inActions.Teleport.GetStateUp(controller))
			{
                TeleportReleased();
			}
		}

        /// <summary>
        /// Add teleport component
        /// </summary>
		void TeleportPressed()
		{
            cp = gameObject.AddComponent<ControllerPointer>();
            cp.UpdateColor(Color.green);
		}

        /// <summary>
        /// Teleport if possible then remove teleport component
        /// </summary>
		void TeleportReleased()
		{
            if(cp.CanTeleport)
            {
                cameraRig.transform.position = cp.TargetPosition;
                cp.DesactivatePointer();
            }
            Destroy(cp);
        }
	}
}


