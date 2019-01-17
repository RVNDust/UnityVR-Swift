using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class FlowPathBehaviour : MonoBehaviour
    {
        public FlowPoint Start, End;
        public GameObject ArrowHead;
        public Vector3 StartSavedPosition = Vector3.zero, EndSavedPosition = Vector3.zero;

        public FlowPathBehaviour(FlowPoint A, FlowPoint B)
        {
            Start = A;
            End = B;
        }

        public void UpdateSavedPositions()
        {
            StartSavedPosition = Start.transform.position;
            EndSavedPosition = End.transform.position;

            if(ArrowHead != null)
                ArrowHead.transform.position = EndSavedPosition;
        }
    }
}
