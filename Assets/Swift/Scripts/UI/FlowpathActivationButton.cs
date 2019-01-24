using Swift.Flow;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift.UI
{
    public class FlowpathActivationButton : ButtonBehaviour
    {
        FlowInformationsBehaviour fib;
        public bool activationState;
        public Color baseColor;

        public Color inactiveColor = Color.gray;

        void Awake()
        {
            AwakeBehaviour();
            activationState = true;
            fib = GetComponentInParent<FlowInformationsBehaviour>();
        }

        void Start()
        {
            StartBehaviour();
            if(baseColor == null)
            {
                baseColor = Color.blue;
            }
        }

        public override void OnClick()
        {
            activationState = !activationState;
            FlowManager.Instance.ToggleSelectedFlowPath(activationState, fib.ProductValue);
            if (activationState)
            {
                btn.GetComponent<Image>().color = baseColor;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
            }
            else
            {
                btn.GetComponent<Image>().color = inactiveColor;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
            }
        }
    }
}


