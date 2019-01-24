using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift.Flow
{
    public class FlowPathBehaviour : MonoBehaviour
    {
        public FlowPoint Start, End;
        public GameObject ArrowHead;
        public Vector3 StartSavedPosition = Vector3.zero, EndSavedPosition = Vector3.zero;

        public FlowPathBehaviour(FlowPoint A, FlowPoint B, GameObject arrow)
        {
            Start = A;
            End = B;
            ArrowHead = arrow;
        }

        public void UpdateSavedPositions()
        {
            if(Start != null && End != null)
            {
                StartSavedPosition = Start.transform.position;
                EndSavedPosition = End.transform.position;
            }

            if(ArrowHead != null)
                ArrowHead.transform.position = EndSavedPosition;
        }
    }
}
