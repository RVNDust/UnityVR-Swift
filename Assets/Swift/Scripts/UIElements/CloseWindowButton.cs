using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
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

        protected override void OnClick()
        {
            window.gameObject.SetActive(false);   
        }
    }
}
