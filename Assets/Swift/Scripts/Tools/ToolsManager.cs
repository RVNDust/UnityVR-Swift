using System.Collections;
using System.Collections.Generic;
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
        public GameObject ToolUIPrefab;
        public float ToolsStartAngle = 180;


        private bool isToolMenuActive = false;
        private Vector2 lastRegisteredPosition;
        private Transform toolCursor;
        private Dictionary<ToolElement, ToolAngleActivation> registeredTools = new Dictionary<ToolElement, ToolAngleActivation>();
        private ToolElement activeTool;
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
            if (SteamVR_Input._default.inActions.ToolsMenu.GetLastStateDown(controller))
            {
                isToolMenuActive = true;
                ToggleToolsMenuDisplay(isToolMenuActive);
                lastRegisteredPosition = SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller);
            }
            else if (SteamVR_Input._default.inActions.ToolsMenu.GetLastStateUp(controller))
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

        void UpdateRadialMenuPosition()
        {
            if (isToolMenuActive)
            {
                float angleVariation = Vector2.SignedAngle(lastRegisteredPosition, SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller));
                lastRegisteredPosition = SteamVR_Input._default.inActions.RadialMenu.GetAxis(controller);
                toolCursor.transform.Rotate(new Vector3(0, 0, 1), angleVariation, Space.Self);
            }
        }

        void ToggleToolsMenuDisplay(bool state)
        {
            ToolsMenu.gameObject.SetActive(state);
        }

        void CreateToolItems()
        {
            if(ToolList.Count > 0)
            {
                float cumulativeOffset = 0;
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

                    cumulativeOffset -= toolActivationAngle;
                }
            }
        }

        void RegisterToolAngle(ToolElement refElement, ToolAngleActivation values)
        {
            registeredTools.Add(refElement, values);
        }

        ToolElement CheckActiveTool()
        {
            float cursorState = toolCursor.GetComponent<RectTransform>().localEulerAngles.z + toolActivationAngle / 2;
            foreach (var tool in registeredTools)
            {
                if(tool.Value.angleStart >= cursorState && cursorState >= tool.Value.angleEnd)
                {
                    return tool.Key;
                }
            }
            return null;
        }

        void ActivateTool()
        {
            if(activeTool != null)
            {
                activeTool.UpdateBehaviour();
                if(activeTool.behaviour != null)
                    activeTool.behaviour.ActivateTool(gameObject);
            }
        }

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
