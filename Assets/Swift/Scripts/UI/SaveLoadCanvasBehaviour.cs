using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Swift
{
    public class SaveLoadCanvasBehaviour : CanvasBehaviour
    {
        public GameObject configContainer;
        public GameObject configButtonPrefab;

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
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
        }

        public void LoadAndDisplayMachineConfigs()
        {
            configContainer.SetActive(true);

            if (!JsonUtils.Instance.IsConfigLoaded)
            {
                JsonUtils.Instance.IsConfigLoaded = true;
                //Gets all the json files in the StreamingAssets/SavedLayout/ repertory
                string[] configFiles = Directory.GetFiles(Application.streamingAssetsPath + "/SavedLayout/", "*.json");
                //For each config file we create a button with the name of the file
                foreach (var filePath in configFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    GameObject newButton = Instantiate(configButtonPrefab);
                    //Set the parent element of the button
                    newButton.transform.SetParent(configContainer.transform, false);
                    newButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
                    newButton.GetComponent<LoadSelectedConfigButton>().fileName = filePath;
                }
            }
        }
    }
}

