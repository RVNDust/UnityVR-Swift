﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace Swift
{
    public class NetworkManagerMultiConfig : NetworkManager
    {
        public GameObject PlayerVR;
        public GameObject PlayerThirdPerson;
        public Vector3 SpawnPoint = Vector3.zero;
        private short customPlayerControllerId = 1;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Called from the client to ask to be added on the shared scene,
        /// Therefore it allows to the client to specify the type of prefab to use ("HTC" or "PC keyboard mouse"
        /// </summary>
        /// <param name="conn"></param>
        public override void OnClientConnect(NetworkConnection conn)
        {
            // Client execution

            // A custom identifier we want to transmit from client to server on connection if there are several users on the same application
            // NOT really used in this case
            short id = customPlayerControllerId++;

            // Create a message to tell to the server which kind of user prefab to spawn
            // the value of AppConfig.Inst.DeviceUsed is "AUTO" (default), "PC" or "HTC"
            StringMessage strMsg = new StringMessage(string.Format("{0};{1}", AppConfig.Inst.DeviceUsed, UnityEngine.XR.XRSettings.isDeviceActive));

            // Call Add player and pass the message
            ClientScene.AddPlayer(conn, id, strMsg);
        }


        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            // Server execution

            string[] msgTypeAndDeviceActive = extraMessageReader.ReadString().Split(';');

            string deviceUsed = msgTypeAndDeviceActive[0];
            bool isVRDeviceActive = msgTypeAndDeviceActive[1].ToLower() == "true";

            Debug.Log(string.Format("OnServerAddPlayer XtraMsg conn:{0} pId:{1} deviceUsed:{2} isVR:{3}", conn.connectionId, playerControllerId, deviceUsed, isVRDeviceActive));

            GameObject prefabToSpawn;

            switch (deviceUsed.ToLower())
            {
                case "htc":
                    prefabToSpawn = PlayerVR;
                    break;

                case "pc":
                    prefabToSpawn = PlayerThirdPerson;
                    break;

                default: // "auto" and others
                    prefabToSpawn = isVRDeviceActive ? PlayerVR : PlayerThirdPerson;
                    break;
            }
            var player = (GameObject)GameObject.Instantiate(prefabToSpawn, SpawnPoint, Quaternion.identity);
            if (autoCreatePlayer) NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            Debug.Log("Client has requested to get his player added to the game " + player);
        }
    }
}