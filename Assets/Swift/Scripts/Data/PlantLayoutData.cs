using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public GameObject PopUpNotif;
        public bool IsConfigLoaded = false;

        GameObject[] GOmachines;
        Animator popupNotifAnim;

        void Awake()
        {
            Instance = this;
            popupNotifAnim = PopUpNotif.GetComponent<Animator>();
        }
        void Start()
        {
            if (GOmachines == null)
            {
                GOmachines = GameObject.FindGameObjectsWithTag("Machine");
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
            JsonUtils.Instance.SaveToJson(Application.dataPath + "/SavedLayouts/", JsonUtils.Instance.GenerateFileName(), machinesJson);
            popupNotifAnim.SetBool("active", true);
            StartCoroutine(addDelay(2f));
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
                RootObject machinesJson = JsonUtility.FromJson<RootObject>(machinesConfig);
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

        IEnumerator addDelay(float delay)
        {
            yield return new WaitForSeconds(2f);
            popupNotifAnim.SetBool("active", false);
        }
    }
}


