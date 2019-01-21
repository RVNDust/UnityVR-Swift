using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Swift
{
    public class RadarCanvasBehaviour : CanvasBehaviour
    {
        GameObject localPlayer;
        List<GameObject> playerOthers = new List<GameObject>();

        public GameObject UserOtherPrefab;

        // Use this for initialization
        void Start()
        {
            StartBehaviour();
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
            GameObject[] playersEntities = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playersEntities)
            {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    localPlayer = player;
                }
                else
                {
                    playerOthers.Add(player);
                }
            }
        }
    }
}

