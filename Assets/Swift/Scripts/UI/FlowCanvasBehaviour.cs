using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift
{
    public class FlowCanvasBehaviour : CanvasBehaviour
    {
        public GameObject container;
        
        public GameObject CreateProductInformations(GameObject informations)
        {
            GameObject go = Instantiate(informations, container.transform);
            FlowpathActivationButton fab = go.GetComponent<FlowInformationsBehaviour>().button.GetComponent<FlowpathActivationButton>();
            fab.GetComponent<Image>().color = fab.inactiveColor;
            fab.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
            fab.activationState = false;
            return go;
        }
    }
}
