using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swift
{
    public class SaveConfigButton : ButtonBehaviour
    {

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
            JsonUtils.Instance.SaveMachineConfigToJson();
        }
    }
}

