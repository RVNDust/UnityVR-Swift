using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift.UI
{
    public class FlowCanvasBehaviour : CanvasBehaviour
    {
        public GameObject container;
        
        public GameObject CreateProductInformations(GameObject informations)
        {
            GameObject go = Instantiate(informations, container.transform);
            return go;
        }
    }
}
