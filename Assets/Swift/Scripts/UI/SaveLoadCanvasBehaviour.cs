using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Swift.UI
{
    public class SaveLoadCanvasBehaviour : CanvasBehaviour
    {
        public ReloadWindowButton ReloadButton;

        public GameObject configContainer;
        public GameObject configButtonPrefab;
        public GameObject popupObject;

        List<GameObject> currentConfigList = new List<GameObject>();
        string layoutPath;

        // Use this for initialization
        void Start()
        {
            StartBehaviour();
            configContainer.SetActive(false);

            ButtonBehaviour[] buttons = GetComponentsInChildren<ButtonBehaviour>();
            foreach (var item in buttons)
            {
                item.gameObject.SetActive(true);
            }

            ReloadButton.onReloadWindow += LoadAndDisplayMachineConfigs;

            ConfigData.SavingPaths sp = ConfigData.Instance.LoadConfigData(ConfigElement.Paths) as ConfigData.SavingPaths;
            layoutPath = sp.Layouts;

            if (!Directory.Exists(Application.dataPath + "/StreamingAssets" + layoutPath))
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets" + layoutPath);
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
        }

        public void LoadAndDisplayMachineConfigs()
        {
            configContainer.SetActive(true);
            foreach (var item in currentConfigList)
            {
                Destroy(item);
            }
            currentConfigList.Clear();
            //Gets all the json files in the StreamingAssets/SavedLayout/ repertory
            string[] configFiles = Directory.GetFiles(Application.dataPath + "/StreamingAssets" + layoutPath, "*.json");
            //For each config file we create a button with the name of the file
            foreach (var filePath in configFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                GameObject newButton = Instantiate(configButtonPrefab);
                //Set the parent element of the button
                newButton.transform.SetParent(configContainer.transform, false);
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
                newButton.GetComponent<LoadSelectedConfigButton>().fileName = filePath;
                currentConfigList.Add(newButton);
            }
        }
    }
}

