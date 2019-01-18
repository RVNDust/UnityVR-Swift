using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift
{
    public class JsonUtils : MonoBehaviour {

        public static JsonUtils Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                    return null;
            }
            private set
            {
                if (instance == null)
                    instance = value;
            }
        }
        private static JsonUtils instance;

        public GameObject PopUpNotif;

        GameObject[] GOmachines;
        GameObject dataManager;
        GameObject saveConfig;
        GameObject loadConfig;
        Animator popupNotifAnim;

        public bool IsConfigLoaded = false;
        private void Awake()
        {
            Instance = this;
            dataManager = GameObject.Find("DataManager");
            saveConfig = GameObject.Find("Button_SaveConfig");
            loadConfig = GameObject.Find("Button_LoadConfig");
            popupNotifAnim = PopUpNotif.GetComponent<Animator>();
        }
        void Start () {
	        if(GOmachines == null)
            {
                GOmachines = GameObject.FindGameObjectsWithTag("Machine");
            }
        }

        /// <summary>
        /// Generates a file name using the following convention : Swift YYYY MM DD – HH mm ss
        /// </summary>
        /// <returns>string fileName</returns>
        string GenerateFileName()
        {
            string fileName = "Swift ";
            DateTime date = DateTime.Now;
            fileName += date.ToString("yyyy MM dd - HH mm ss");
            fileName += ".json";
            return fileName;
        }

        /// <summary>
        /// Save the machines positions & rotation in a Json File
        /// </summary>
        public void SaveMachineConfigToJson()
        {
            string JsonToSave = "";
            string fileName = GenerateFileName();
            string filePath = Application.dataPath + "/StreamingAssets/SavedLayout/" + fileName;

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
            JsonToSave = JsonUtility.ToJson(machinesJson);
            File.WriteAllText(filePath, JsonToSave);
            popupNotifAnim.SetBool("active", true);
            StartCoroutine(addDelay(2f));
        }

        IEnumerator addDelay(float delay)
        {
            yield return new WaitForSeconds(2f);
            popupNotifAnim.SetBool("active", false);
        }

        /// <summary>
        /// Load the selected file and move the machines
        /// </summary>
        public void LoadSelectedMachineConfig(string filePath)
        { 
           if (File.Exists(filePath))
            {
                //read the json file and put it in dataAsJson
                string dataAsJson = File.ReadAllText(filePath);
                //Pass the json to JsonUtility and create a RootObject (the list of every machines in the savefile)
                RootObject machinesJson = JsonUtility.FromJson<RootObject>(dataAsJson);
                //For each machine saved we change the Pos/Rot values of the corresponding GameObject
                foreach (var machine in machinesJson.machinesList)
                {
                    var tempMachine = GameObject.Find(machine.MachineName);
                    tempMachine.transform.position = machine.MachinePosition;
                    tempMachine.transform.rotation = machine.MachineRotation;
                }
            }
            else
            {
                Debug.Log("Path given not found");
            }
        }

    }

    [Serializable]
    public class Machine
    {
        public string MachineName;
        public Vector3 MachinePosition;
        public Quaternion MachineRotation;
    }

    [Serializable]
    public class RootObject
    {
        public List<Machine> machinesList;
    }
}
