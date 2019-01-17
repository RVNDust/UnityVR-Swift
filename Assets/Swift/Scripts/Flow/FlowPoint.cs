using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class FlowPoint : MonoBehaviour
    {
        public FlowpointState State;
        
    }

    public enum FlowpointState
    {
        Enter,
        Exit
    }
}
