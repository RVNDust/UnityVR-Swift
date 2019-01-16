using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class FlowCanvasBehaviour : MonoBehaviour
    {
        public GameObject container;
        
        public void CreateProductInformations(GameObject informations)
        {
            Instantiate(informations, container.transform);
        }
    }
}
