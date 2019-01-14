using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Swift
{
    public class JsonUtils : MonoBehaviour {

        GameObject[] machines;

	    // Use this for initialization
	    void Start () {
	        if(machines == null)
            {
                machines = GameObject.FindGameObjectsWithTag("Machine");
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

        public void SaveMachineConfigToJson()
        {
            string JsonToSave = "";
            string fileName = GenerateFileName();
            string filePath = Application.dataPath + "/StreamingAssets/SavedLayout/" + fileName;

            RootObject machinesJson = new RootObject();
            machinesJson.machinesList = new List<Machine>();

            foreach (GameObject machine in machines)
            {
                Machine currentMachine = new Machine();
                currentMachine.MachineName = machine.name;
                currentMachine.MachinePosition = machine.transform.position;
                currentMachine.MachineRotation = machine.transform.rotation;

                machinesJson.machinesList.Add(currentMachine);
            }
            
            JsonToSave = JsonUtility.ToJson(machinesJson);
            File.WriteAllText(filePath, JsonToSave);
        }

        public void LoadMachineConfigFromJson()
        {

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
