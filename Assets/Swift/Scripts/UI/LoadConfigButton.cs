using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swift
{
    public class LoadConfigButton : ButtonBehaviour
    {
        CanvasBehaviour window;
        
        // Use this for initialization
        void Start()
        {
            StartBehaviour();
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
            window = GetComponentInParent<CanvasBehaviour>();
        }

        protected override void OnClick()
        {
            //btn.interactable = false;
            window.GetComponent<SaveLoadCanvasBehaviour>().LoadAndDisplayMachineConfigs();
        }
    }
}