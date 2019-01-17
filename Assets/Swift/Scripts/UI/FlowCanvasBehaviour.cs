using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class FlowCanvasBehaviour : MonoBehaviour
    {
        public GameObject container;
        
        public GameObject CreateProductInformations(GameObject informations)
        {
            return Instantiate(informations, container.transform);
        }
    }
}
