using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Swift
{
    [CreateAssetMenu(fileName = "ToolElement", menuName = "Swift/Tool/New Tool", order = 1)]
    public class ToolElement : ScriptableObject
    {
        public string name;
        public Sprite icon;
        public GameObject behaviourContainer;
        public string behaviourName;
        [HideInInspector] public ToolBehaviour behaviour;

        public void UpdateBehaviour()
        {
            if(behaviourContainer != null)
            {
                behaviour = behaviourContainer.GetComponent<ToolBehaviour>();
            }
        }
    }
}
