using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class FlowManager : MonoBehaviour
    {
        public List<GameObject> FlowpathA = new List<GameObject>();
        public List<GameObject> FlowpathB = new List<GameObject>();
        public List<GameObject> FlowpathC = new List<GameObject>();
        public List<GameObject> FlowpathD = new List<GameObject>();
        public List<GameObject> FlowpathE = new List<GameObject>();

        public Dictionary<Product, List<GameObject>> productFlows = new Dictionary<Product, List<GameObject>>();

        // Use this for initialization
        void Start()
        {
            CreateFlowPath(FlowpathA, Product.A);
            CreateFlowPath(FlowpathB, Product.B);
            CreateFlowPath(FlowpathC, Product.C);
            CreateFlowPath(FlowpathD, Product.D);
            CreateFlowPath(FlowpathE, Product.E);
            ToggleDisplayFlowPath(true);
        }

        public void CreateFlowPath(List<GameObject> flowpathList, Product productType)
        {
            FlowPoint lastFlowpoint = null;
            foreach (GameObject item in flowpathList)
            {
                FlowPoint[] flowpoints = item.GetComponentsInChildren<FlowPoint>();
                FlowPoint enter = null, exit = null;
                for (int i = 0; i < flowpoints.Length; i++)
                {
                    if (flowpoints[i].State == FlowpointState.Exit)
                    {
                        exit = flowpoints[i];
                    }
                    else if(flowpoints[i].State == FlowpointState.Enter)
                    {
                        enter = flowpoints[i];
                    }
                }
                if (lastFlowpoint != null)
                {
                    GameObject flowpath = new GameObject();
                    LineRenderer lr = flowpath.AddComponent<LineRenderer>();
                    Vector3[] flowpathPositions = {enter.transform.position, lastFlowpoint.transform.position};
                    lr.SetPositions(flowpathPositions);
                    lr.startColor = Color.magenta;
                    lr.endColor = Color.magenta;
                    lr.startWidth = 0.15f;
                    lr.endWidth = 0.15f;

                    if (!productFlows.ContainsKey(productType))
                        productFlows.Add(productType, new List<GameObject>());

                    productFlows[productType].Add(flowpath);
                }
                lastFlowpoint = exit;
            }
        }

        public void ToggleDisplayFlowPath(bool state)
        {
            foreach (var item in productFlows)
            {
                foreach (var flowpath in item.Value)
                {
                    flowpath.SetActive(state);
                }
            }
        }
    }

    public enum Product
    {
        A,
        B,
        C,
        D,
        E
    }
}
