using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class FlowManager : MonoBehaviour
    {
        public static FlowManager Instance {
            get
            {
                if (instance != null)
                    return instance;
                else
                    return null;
            }
            private set
            {
                if (instance == null)
                    instance = value;
            }
        }
        private static FlowManager instance;

        public bool IsFilled = false;
        public GameObject arrowRef;

        public List<GameObject> FlowpathA = new List<GameObject>();
        public List<GameObject> FlowpathB = new List<GameObject>();
        public List<GameObject> FlowpathC = new List<GameObject>();
        public List<GameObject> FlowpathD = new List<GameObject>();
        public List<GameObject> FlowpathE = new List<GameObject>();

        public Dictionary<string, List<GameObject>> productFlows = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, Color> productColor = new Dictionary<string, Color>(); //TODO Change with configurable values in JSON config file

        public Material baseMaterial;

        private GameObject flowsContainer;

        // Use this for initialization
        void Start()
        {
            Instance = this;
            flowsContainer = new GameObject("FlowsContainer");
            StartCoroutine(Delays());
        }

        private void LoadFlowsData()
        {
            ConfigData.Flows flowsData = ConfigData.Instance.LoadConfigData(ConfigElement.Flows) as ConfigData.Flows;
            foreach (var product in flowsData.Products)
            {
                //Handling color of each product
                Color color;
                ColorUtility.TryParseHtmlString(product.Color, out color);
                productColor.Add(product.Name, color);

                //Handling connexion with the different machines
                List<GameObject> tempFlowpath = new List<GameObject>();
                foreach (var machine in product.Machines)
                {
                    Debug.Log(machine);
                    GameObject machineGo = GameObject.Find(machine);
                    tempFlowpath.Add(machineGo);
                }
                Debug.Log(tempFlowpath);
                CreateFlowPath(tempFlowpath, product.Name);
            }

            ToggleDisplayFlowPath(true);

            StartCoroutine(AutoRefreshFlows());
        }

        public void CreateFlowPath(List<GameObject> flowpathList, string productType)
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
                ToggleSelectedFlowPath(state, item.Key);
            }
        }

        public void ToggleSelectedFlowPath(bool state, string product)
        {
            foreach (var flowpath in productFlows[product])
            {
                flowpath.SetActive(state);
            }
        }

        IEnumerator AutoRefreshFlows()
        {
            UpdateFlowPath();
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(AutoRefreshFlows());
        }

        IEnumerator Delays()
        {
            yield return new WaitForSeconds(5.0f);
            LoadFlowsData();
        }
    }
}
