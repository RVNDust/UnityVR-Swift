using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift.UI
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

        public override void OnClick()
        {
            if (onReloadWindow != null)
                onReloadWindow();
        }
    }
}

