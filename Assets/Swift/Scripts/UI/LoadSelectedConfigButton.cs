using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift.UI
{
    public class LoadSelectedConfigButton : ButtonBehaviour
    {
        public string fileName;

        // Use this for initialization
        void Start()
        {
            StartBehaviour();
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
        }

        public override void OnClick()
        {
            base.OnClick();
            Debug.Log("LoadConfig");
            PlantLayoutData.Instance.LoadSelectedMachineConfig(fileName);
        }
    }
}
