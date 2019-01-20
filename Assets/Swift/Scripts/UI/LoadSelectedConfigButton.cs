using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
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

        protected override void OnClick()
        {
            base.OnClick();
            PlantLayoutData.Instance.LoadSelectedMachineConfig(fileName);
        }
    }
}
