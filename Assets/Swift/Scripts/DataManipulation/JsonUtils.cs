using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Swift
{
    public class JsonUtils : MonoBehaviour {

        public Button button;
        public Canvas canvas;
        GameObject[] GOmachines;

	    // Use this for initialization
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
        }

        /// <summary>
        /// Display them on the selection pannel
        /// </summary>
        public void LoadAndDisplayMachineConfigs()
        {
            //Gets all the json files in the StreamingAssets/SavedLayout/ repertory
            string[] configFiles = Directory.GetFiles(Application.streamingAssetsPath + "/SavedLayout/", "*.json");
            foreach (var fileName in configFiles)
            {
                Button newButton = Instantiate(button) as Button;
                newButton.transform.SetParent(canvas.transform, false);
                Debug.Log(Path.GetFileNameWithoutExtension(fileName));
            }
            //TODO foreach files, instanciate [Button/text = file.name] in MachineConfigSelectZone
        }

        public void CreateButton(Transform panel, Vector3 position, Vector2 size, UnityEngine.Events.UnityAction method)
        {
            GameObject button = new GameObject();
            button.transform.parent = panel;
            button.AddComponent<RectTransform>();
            button.AddComponent<Button>();
            button.transform.position = position;
            //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(size);
            button.GetComponent<Button>().onClick.AddListener(method);
        }

        /// <summary>
        /// Load the selected file and move machines
        /// </summary>
        public void LoadSelectedMachineConfig(string fileName)
        {
            Debug.Log("Hello work, " + fileName);
            //TODO open the file, put the datas in the classes, foreach gameobjectWithTag("Machine") change Transform/Rotation
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
