using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swift
{
    public class TargetStateCanvasBehaviour : CanvasBehaviour
    {
        string folderPath = Application.dataPath;
        string fileName = "/configFile.json";

        Image img;

        // Use this for initialization
        void Start()
        {
            StartBehaviour();
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
            img = GetComponentInChildren<Image>();
        }

        void Update()
        {

        }
    }
}
