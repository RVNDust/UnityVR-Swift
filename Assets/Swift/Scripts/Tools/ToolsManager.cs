using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace Swift
{
    public class ToolsManager : MonoBehaviour
    {
        public SteamVR_Input_Sources controller;
        public List<ToolElement> ToolList = new List<ToolElement>();
        public Canvas ToolsMenu;
        public TextMeshProUGUI toolName;
        public GameObject ToolUIPrefab;
        public float ToolsStartAngle = 180;


        private bool isToolMenuActive = false;
        private Vector2 lastRegisteredPosition;
        private Transform toolCursor;
        private Dictionary<ToolElement, ToolAngleActivation> registeredTools = new Dictionary<ToolElement, ToolAngleActivation>();
        private Dictionary<ToolElement, Animator> animators = new Dictionary<ToolElement, Animator>();
        private ToolElement activeTool, lastAnimated;
        private float toolActivationAngle;

        // Use this for initialization
        void Start()
        {
            controller = gameObject.GetComponent<SteamVR_Behaviour_Pose>().inputSource;
            ToolsMenu = GetComponentInChildren<Canvas>();
            toolCursor = ToolsMenu.transform.Find("RadialSelector");

            toolActivationAngle = 360 / ToolList.Count;
            CreateToolItems();
            ToggleToolsMenuDisplay(false);
        }

        void Update()
        {
            if (SteamVR_Input._default.inActions.ToolsMenu.GetStateDown(controller))
            {
                isToolMenuActive = true;
                ToggleToolsMenuDisplay(isToolMenuActive);
                lastRegisteredPosition = SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller);
            }
            else if (SteamVR_Input._default.inActions.ToolsMenu.GetStateUp(controller))
            {
                ToolElement currentTool = CheckActiveTool();
                if (currentTool != activeTool)
                {
                    DesactivateTool();
                    activeTool = currentTool;
                    ActivateTool();
                }
                isToolMenuActive = false;
                ToggleToolsMenuDisplay(isToolMenuActive);
                lastRegisteredPosition = Vector2.zero;
            }
            UpdateRadialMenuPosition();
        }

        /// <summary>
        /// Update position of tool selector gameobject
        /// </summary>
        void UpdateRadialMenuPosition()
        {
            if (isToolMenuActive)
            {
                float angleVariation = Vector2.SignedAngle(lastRegisteredPosition, SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller));
                lastRegisteredPosition = SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller);
                toolCursor.transform.Rotate(new Vector3(0, 0, 1), angleVariation, Space.Self);
            }

            if(toolCursor.transform.localRotation.z > 180)
            {
                toolCursor.transform.Rotate(new Vector3(0, 0, 1), -360, Space.Self);
            }
            else if (toolCursor.transform.localRotation.z < -180)
            {
                toolCursor.transform.Rotate(new Vector3(0, 0, 1), 360, Space.Self);
            }

            ToolElement currentTool = CheckActiveTool();
            if(lastAnimated != currentTool)
            {
                if(lastAnimated != null)
                    animators[lastAnimated].SetBool("IsHovered", false);
                animators[currentTool].SetBool("IsHovered", true);
                lastAnimated = currentTool;
            }
        }

        /// <summary>
        /// Activate and deactivate displays of item menu canvas
        /// </summary>
        /// <param name="state">new state of the canvas</param>
        void ToggleToolsMenuDisplay(bool state)
        {
            ToolsMenu.gameObject.SetActive(state);
        }

        /// <summary>
        /// Used to create a menu item
        /// </summary>
        void CreateToolItems()
        {
            if(ToolList.Count > 0)
            {
                float cumulativeOffset = 180;
                float angleOffset = 1.0f / ToolList.Count;
                toolCursor.GetComponent<Image>().fillAmount = angleOffset;
                foreach (ToolElement item in ToolList)
                {
                    GameObject toolItem = Instantiate(ToolUIPrefab, ToolsMenu.transform);
                    toolItem.name = item.name;
                    toolItem.GetComponentInChildren<Image>().fillAmount = angleOffset - 0.02f;
                    toolItem.transform.Rotate(new Vector3(0, 0, 1), cumulativeOffset, Space.Self);
                    toolItem.GetComponentInChildren<ToolInfoBehaviour>().UpdateInfo(item.name, item.icon);
                    toolItem.GetComponentInChildren<ToolInfoBehaviour>().UpdateRotation(cumulativeOffset);

                    RegisterToolAngle(item, new ToolAngleActivation(ToolsStartAngle, ToolsStartAngle -= toolActivationAngle)); 
                    animators.Add(item, toolItem.GetComponent<Animator>());

                    cumulativeOffset -= toolActivationAngle;
                }
            }
        }

        /// <summary>
        /// Used to register angles between which tool will be activated
        /// </summary>
        /// <param name="refElement"></param>
        /// <param name="values"></param>
        void RegisterToolAngle(ToolElement refElement, ToolAngleActivation values)
        {
            registeredTools.Add(refElement, values);
        }

        /// <summary>
        /// Check which tool is considered active
        /// </summary>
        /// <returns></returns>
        ToolElement CheckActiveTool()
        {
            float cursorState = toolCursor.GetComponent<RectTransform>().localEulerAngles.z - toolActivationAngle / 2;
            foreach (var tool in registeredTools)
            {
                if(tool.Value.angleStart >= cursorState && cursorState > tool.Value.angleEnd)
                {
                    Debug.Log("Cursor z rotation: " + toolCursor.GetComponent<RectTransform>().localEulerAngles.z + "\ncursorState: " + cursorState + "\nangleStart: " + tool.Value.angleStart + "\nangleEnd: " + tool.Value.angleEnd);
                    return tool.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// Active the tool referenced in activeTool variable
        /// </summary>
        void ActivateTool()
        {
            if(activeTool != null)
            {
                activeTool.UpdateBehaviour();
                if (activeTool.behaviour != null)
                {
                    activeTool.behaviour.ActivateTool(gameObject);
                    toolName.text = activeTool.name;
                }
            }
        }

        /// <summary>
        /// Deactivate the tool referenced, if there is one, in activeTool variable
        /// </summary>
        void DesactivateTool()
        {
            if(activeTool != null)
            {
                activeTool.UpdateBehaviour();
                if (activeTool.behaviour != null)
                    activeTool.behaviour.DesactivateTool(gameObject);
            }
        }
    }

    struct ToolAngleActivation
    {
        public float angleStart;
        public float angleEnd;

        public ToolAngleActivation(float start, float end)
        {
            angleStart = start;
            angleEnd = end;
        }
    }
}
