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
        public float ToolsStartAngle = 0;
        public SteamVR_Action_Vibration haptics;

        private bool isToolMenuActive = false;
        private bool isChangingTool = false;
        private Vector2 lastRegisteredPosition;
        private float currentAngle = 0;
        private float lastRegisteredAngle = 0;
        private Transform toolCursor;
        private Dictionary<ToolElement, ToolAngleActivation> registeredTools = new Dictionary<ToolElement, ToolAngleActivation>();
        private Dictionary<ToolElement, Animator> animators = new Dictionary<ToolElement, Animator>();
        private ToolElement activeTool, hoveredTool;
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
            if (SteamVR_Input._default.inActions.Touchpad.GetStateDown(controller))
                isChangingTool = true;
            else if (SteamVR_Input._default.inActions.Touchpad.GetStateUp(controller))
                isChangingTool = false;

            if (SteamVR_Input._default.inActions.ToolsMenu.GetStateDown(controller))
            {
                isToolMenuActive = true;
                ToggleToolsMenuDisplay(isToolMenuActive);
                lastRegisteredPosition = SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller);
            }
            else if (SteamVR_Input._default.inActions.ToolsMenu.GetStateUp(controller))
            {
                ToolElement currentTool = CheckActiveTool();
                if (currentTool != null)
                {
                    DesactivateTool();
                    if (activeTool != currentTool)
                        haptics.Execute(0, .05f, 65, .25f, controller);
                    activeTool = currentTool;
                    ActivateTool();
                }
                isToolMenuActive = false;
                ToggleToolsMenuDisplay(isToolMenuActive);
                lastRegisteredPosition = Vector2.zero;
            }
            if(isToolMenuActive)
                UpdateRadialMenuPosition();
        }

        /// <summary>
        /// Update position of tool selector gameobject
        /// </summary>
        void UpdateRadialMenuPosition()
        {
            if (isToolMenuActive && isChangingTool)
            {
                lastRegisteredPosition = SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller);
                CalculateCurrentAngle();
                toolCursor.transform.localRotation = Quaternion.Euler(0,0, currentAngle + toolActivationAngle / 2);
            }

            ToolElement currentTool = CheckActiveTool();
            if(hoveredTool != null && currentTool != hoveredTool)
            {
                haptics.Execute(0, 0.05f, 50, 0.3f, controller);
            }
            hoveredTool = currentTool;
            foreach (var item in animators)
            {
                if(item.Key != currentTool)
                {
                    animators[item.Key].SetBool("IsHovered", false);
                }
                else
                {
                    toolName.text = item.Key.name;
                    animators[item.Key].SetBool("IsHovered", true);
                }
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
                float angleOffset = 1.0f / ToolList.Count;
                toolCursor.GetComponent<Image>().fillAmount = angleOffset;
                foreach (ToolElement item in ToolList)
                {
                    GameObject toolItem = Instantiate(ToolUIPrefab, ToolsMenu.transform);
                    toolItem.name = item.name;
                    toolItem.GetComponentInChildren<Image>().fillAmount = angleOffset - 0.01f;
                    toolItem.transform.Rotate(new Vector3(0, 0, 1), ToolsStartAngle, Space.Self);
                    toolItem.GetComponentInChildren<ToolInfoBehaviour>().UpdateInfo(item.name, item.icon);
                    toolItem.GetComponentInChildren<ToolInfoBehaviour>().UpdateRotation(-ToolsStartAngle);

                    RegisterToolAngle(item, new ToolAngleActivation(ToolsStartAngle, ToolsStartAngle -= toolActivationAngle)); 
                    animators.Add(item, toolItem.GetComponent<Animator>());
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

        void CalculateCurrentAngle()
        {
            currentAngle = -((Mathf.Atan2(lastRegisteredPosition.x, lastRegisteredPosition.y) / Mathf.PI) * 180f);
            if(currentAngle < -360f)
            {
                currentAngle += 360f;
            }
            else if(currentAngle > 0)
            {
                currentAngle -= 360f;
            }
        }

        /// <summary>
        /// Check which tool is considered active
        /// </summary>
        /// <returns></returns>
        ToolElement CheckActiveTool()
        {
            float cursorState = currentAngle; //- toolActivationAngle / 2;
            foreach (var tool in registeredTools)
            {
                if(tool.Value.angleStart >= cursorState && cursorState > tool.Value.angleEnd)
                {
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
