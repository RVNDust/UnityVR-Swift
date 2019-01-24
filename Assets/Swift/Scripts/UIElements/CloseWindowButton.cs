using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift.UI
{
    public class CloseWindowButton : ButtonBehaviour
    {
        CanvasBehaviour window;

        void Awake()
        {
            AwakeBehaviour();
            window = GetComponentInParent<CanvasBehaviour>();
        }

        void Start()
        {
            StartBehaviour();
        }

        public override void OnClick()
        {
            window.gameObject.SetActive(false);   
        }
    }
}
