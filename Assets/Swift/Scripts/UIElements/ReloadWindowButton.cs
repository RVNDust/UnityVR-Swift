using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class ReloadWindowButton : ButtonBehaviour {

        CanvasBehaviour window;

        public delegate void OnWindowEvent();
        public OnWindowEvent onReloadWindow;

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
            if (onReloadWindow != null)
                onReloadWindow();
        }
    }
}

