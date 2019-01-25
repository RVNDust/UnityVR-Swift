using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swift.UI
{
    public class SaveConfigButton : ButtonBehaviour
    {
        public Animator popupNotifRef;

        SaveLoadCanvasBehaviour window;
        // Use this for initialization
        void Start()
        {
            StartBehaviour();
            window = GetComponentInParent<SaveLoadCanvasBehaviour>();
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
        }

        public override void OnClick()
        {
            PlantLayoutData.Instance.SaveMachineConfigToJson();
            if (popupNotifRef != null)
                StartCoroutine(PopupDelay());
            if (window != null)
                window.LoadAndDisplayMachineConfigs();
        }

        IEnumerator PopupDelay()
        {
            popupNotifRef.SetBool("active", true);
            yield return new WaitForSeconds(.5f);
            popupNotifRef.SetBool("active", false);
        }
    }
}

