using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Swift.Data
{
    public class PlantLayoutData : MonoBehaviour
    {
        [Serializable]
        public class RootObject
        {
            public List<Machine> machinesList;
        }

        [Serializable]
        public class Machine
        {
            public string MachineName;
            public Vector3 MachinePosition;
            public Quaternion MachineRotation;
        }

        public static PlantLayoutData Instance { get; private set; }
        public GameObject vrPlayer;
        [HideInInspector] public bool IsConfigLoaded = false;

        GameObject[] GOmachines;
        Animator popupNotifAnim;
        string layoutPath;
        GameObject localPlayer;

        void GetLocalPlayer()
        {
            GameObject[] playersEntities = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playersEntities)
            {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    localPlayer = player;
                }
            }
        }

        void Awake()
        {
            Instance = this;
            //popupNotifAnim = PopUpNotif.GetComponent<Animator>();
        }
        void Start()
        {
            ConfigData.SavingPaths sp = ConfigData.Instance.LoadConfigData(ConfigElement.Paths) as ConfigData.SavingPaths;
            layoutPath = sp.Layouts;
        }
        
        private GameObject GetPlayerReference()
        {
            GameObject[] playersEntities = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playersEntities)
            {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    vrPlayer = player;
                    return player;
                }
            }
            return null;
        }

        void Update()
        {
            if (localPlayer != null)
            {
                if (GOmachines == null)
                {
                    GOmachines = GameObject.FindGameObjectsWithTag("Machine");
                }
            }
            else
            {
                localPlayer = GetPlayerReference();
            }
        }

        /// <summary>
        /// Save the machines positions & rotation in a Json File
        /// </summary>
        public void SaveMachineConfigToJson()
        {
            RootObject machinesJson = new RootObject();
            machinesJson.machinesList = new List<Machine>();

            foreach (GameObject machine in GOmachines)
            {
                Machine currentMachine = new Machine();
                currentMachine.MachineName = machine.name;
                currentMachine.MachinePosition = machine.transform.position;
                currentMachine.MachineRotation = machine.transform.rotation;

                machinesJson.machinesList.Add(currentMachine);
            }
            JsonUtils.Instance.SaveToJson(Application.dataPath + "/StreamingAssets" + layoutPath, JsonUtils.Instance.GenerateFileName(), machinesJson);
        }

        /// <summary>
        /// Load the selected file and move the machines
        /// </summary>
        public void LoadSelectedMachineConfig(string filePath)
        {
            string machinesConfig = JsonUtils.Instance.LoadFromJson(filePath);
            if (machinesConfig != "")
            {
                //Pass the json to JsonUtility and create a RootObject (the list of every machines in the savefile)
                localPlayer.GetComponent<VR_CameraRigMultiuser>().CmdLoadLayoutConfiguration(machinesConfig);
            }
            else
            {
                Debug.Log("Path given not found");
            }
        }

        IEnumerator addDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}


