using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

namespace Swift.Tools
{
    public class ToolTopViewTPS : MonoBehaviour
    {
        public GameObject TopViewPlane;
        public float YOffset;

        ViewMode mode = ViewMode.Normal;
        GameObject player;

        void Start()
        {
            GameObject[] playersEntities = GameObject.FindGameObjectsWithTag("Player");
            foreach (var p in playersEntities)
            {
                if (p.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    player = p;
                }
            }

            if(TopViewPlane == null)
            {
                TopViewPlane = transform.Find("/TopViewPlane").gameObject;
            }
        }

        void Update()
        {
            if(player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                if (Input.GetButtonUp("ToggleCamera"))
                {
                    TogglePlan();
                }
            }
        }


        private void TogglePlan()
        {
            Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            //Toggle between plan
            switch (mode)
            {
                case ViewMode.Normal:
                    mode = ViewMode.Top;
                    pos.y = TopViewPlane.transform.position.y + YOffset;
                    player.transform.position = pos;
                    break;
                case ViewMode.Top:
                    mode = ViewMode.Normal;
                    pos.y = 0;
                    player.transform.position = pos;
                    break;
            }
        }
    }
}

