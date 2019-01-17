using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class FlowManager : MonoBehaviour
    {
        public bool IsFilled = false;
        public GameObject arrowRef;

        public List<GameObject> FlowpathA = new List<GameObject>();
        public List<GameObject> FlowpathB = new List<GameObject>();
        public List<GameObject> FlowpathC = new List<GameObject>();
        public List<GameObject> FlowpathD = new List<GameObject>();
        public List<GameObject> FlowpathE = new List<GameObject>();

        public Dictionary<Product, List<GameObject>> productFlows = new Dictionary<Product, List<GameObject>>();
        public Dictionary<Product, Color> productColor = new Dictionary<Product, Color>(); //TODO Change with configurable values in JSON config file

        public Material baseMaterial;

        private GameObject flowsContainer;

        // Use this for initialization
        void Start()
        {
            flowsContainer = new GameObject("FlowsContainer");

            productColor.Add(Product.A, Color.blue);
            productColor.Add(Product.B, Color.green);
            productColor.Add(Product.C, Color.red);
            productColor.Add(Product.D, Color.yellow);
            productColor.Add(Product.E, Color.magenta);
            
            CreateFlowPath(FlowpathA, Product.A);
            CreateFlowPath(FlowpathB, Product.B);
            CreateFlowPath(FlowpathC, Product.C);
            CreateFlowPath(FlowpathD, Product.D);
            CreateFlowPath(FlowpathE, Product.E);
            ToggleDisplayFlowPath(false);

            StartCoroutine(AutoRefreshFlows());
        }


        public void CreateFlowPath(List<GameObject> flowpathList, Product productType)
        {
            FlowPoint lastFlowpoint = null;
            Color flowColor = productColor[productType];
            Material mat = new Material(baseMaterial);
            mat.color = flowColor;
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
                    GameObject flowpath = new GameObject("Flowpath");
                    flowpath.transform.parent = flowsContainer.transform;
                    FlowPathBehaviour fb = flowpath.AddComponent<FlowPathBehaviour>();
                    fb.Start = lastFlowpoint;
                    fb.End = enter;
                    fb.UpdateSavedPositions();
                    LineRenderer lr = flowpath.AddComponent<LineRenderer>();
                    Vector3[] flowpathPositions = {enter.transform.position, lastFlowpoint.transform.position};
                    lr.SetPositions(flowpathPositions);
                    lr.material = mat;
                    lr.startWidth = 0.15f;
                    lr.endWidth = 0.15f;

                    //Creating the head of the arrow
                    Instantiate(arrowRef, flowpath.transform);
                    arrowRef.transform.position = lastFlowpoint.transform.position;
                    arrowRef.GetComponent<SpriteRenderer>().material = mat;
                    fb.ArrowHead = arrowRef;

                    if (!productFlows.ContainsKey(productType))
                        productFlows.Add(productType, new List<GameObject>());

                    productFlows[productType].Add(flowpath);
                }
                lastFlowpoint = exit;
            }
        }

        public void UpdateFlowPath()
        {
            foreach (var item in productFlows)
            {
                foreach (var flowpath in item.Value)
                {
                    FlowPathBehaviour fb = flowpath.GetComponent<FlowPathBehaviour>();
                    fb.UpdateSavedPositions();
                    LineRenderer lr = flowpath.GetComponent<LineRenderer>();
                    Vector3[] flowpathPositions = { fb.StartSavedPosition, fb.EndSavedPosition};
                    lr.SetPositions(flowpathPositions);
                }
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

        IEnumerator AutoRefreshFlows()
        {
            UpdateFlowPath();
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(AutoRefreshFlows());
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
