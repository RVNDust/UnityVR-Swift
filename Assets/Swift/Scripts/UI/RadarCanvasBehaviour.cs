﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Swift
{
    public class RadarCanvasBehaviour : CanvasBehaviour
    {
        GameObject localPlayer;
        List<GameObject> playerOthers = new List<GameObject>();
        Dictionary<GameObject, GameObject> playerOthersRepresentation = new Dictionary<GameObject, GameObject>();
        GameObject networkManager;
        int numPlayers = 0;

        public GameObject UserOtherPrefab;
        public GameObject UserOtherContainer;
        public float RadarRange = 10.0f;
        public float ScaleFactor = 10.0f;
        public Color inRange, outOfRange;

        // Use this for initialization
        void Start()
        {
            StartBehaviour();
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
            networkManager = GameObject.Find("NetworkManager");
            if (networkManager == null)
                Debug.Log("No network manager found");

            GetAllPlayers();

            networkManager.GetComponent<NetworkManagerMultiConfig>().onServerAddPlayerEvent += RadarCanvasBehaviour_onServerAddPlayerEvent;
            networkManager.GetComponent<NetworkManagerMultiConfig>().onServerRemovePlayerEvent += RadarCanvasBehaviour_onServerRemovePlayerEvent;

            StartCoroutine(UpdateRadar());
        }

        private void RadarCanvasBehaviour_onServerRemovePlayerEvent(GameObject playerRef)
        {
            if(playerOthers.Contains(playerRef))
                playerOthers.Remove(playerRef);
        }

        private void RadarCanvasBehaviour_onServerAddPlayerEvent(GameObject playerRef)
        {
            Debug.Log("Event Ok");
            playerOthers.Add(playerRef);
        }

        public IEnumerator UpdateRadar()
        {
            yield return new WaitForSeconds(.1f);
            if(numPlayers != networkManager.GetComponent<NetworkManagerMultiConfig>().numPlayers)
            {
                numPlayers = networkManager.GetComponent<NetworkManagerMultiConfig>().numPlayers;
                GetAllPlayers();
                List<GameObject> toRemove = new List<GameObject>();
                foreach (var item in playerOthersRepresentation)
                {
                    if(!playerOthers.Contains(item.Key))
                    {
                        Destroy(item.Value);
                        toRemove.Add(item.Key);
                    }
                }
                foreach (var item in toRemove)
                {
                    playerOthersRepresentation.Remove(item);
                }
                toRemove.Clear();
            }
            foreach (var player in playerOthers)
            {
                UpdatePlayerPositions(player);
            }
            StartCoroutine(UpdateRadar());
        }
        
        private void UpdatePlayerPositions(GameObject userOther)
        {
            //Get the distance between localPlayer and the player reference
            float distanceDiff = Vector3.Distance(localPlayer.transform.position, userOther.transform.position);
            float posX, posY; //Position of the representation (userOther)
            CreateUserOtherRepresentation(userOther);
            //Test if player reference is in the circle (change the "state")
            if (distanceDiff > RadarRange)
            {
                //UserOther is around the radar
                float angleUserOther = Vector3.Angle(localPlayer.transform.position, userOther.transform.position);
                posX = 105;
                posY = 0;
                playerOthersRepresentation[userOther].GetComponentInChildren<Image>().color = outOfRange;
                playerOthersRepresentation[userOther].GetComponentInChildren<Image>().transform.localPosition = new Vector3(posX, posY, 0);
                playerOthersRepresentation[userOther].GetComponentInChildren<Image>().transform.localRotation = Quaternion.Euler(new Vector3(0,0,90));
                playerOthersRepresentation[userOther].transform.Rotate(Vector3.forward, angleUserOther, Space.Self);
            }
            else
            {
                //UserOther is in the circle
                Vector3 posDiff = userOther.transform.position - localPlayer.transform.position;
                posX = posDiff.x * ScaleFactor;
                posY = posDiff.z * ScaleFactor;
                playerOthersRepresentation[userOther].GetComponentInChildren<Image>().color = inRange;
                playerOthersRepresentation[userOther].GetComponentInChildren<Image>().transform.localPosition = new Vector3(posX, posY, 0);
                playerOthersRepresentation[userOther].GetComponentInChildren<Image>().transform.localRotation = Quaternion.Euler(new Vector3(0,0, userOther.transform.rotation.y));
            }

        }

        private void CreateUserOtherRepresentation(GameObject userOther)
        {
            if (!playerOthersRepresentation.ContainsKey(userOther))
            {
                //Create userOther representation
                GameObject tmp = Instantiate(UserOtherPrefab, UserOtherContainer.transform);
                playerOthersRepresentation.Add(userOther, tmp);
            }
        }

        private void GetAllPlayers()
        {
            //Find all the clients initially connected, before radar is opened
            playerOthers.Clear();
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

