using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift
{
    public class LookAt : MonoBehaviour
    {
        public Transform target;

        void Start()
        {
            if (target == null)
                target = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(target);
            transform.Rotate(new Vector3(0, 1, 0), 180);
        }
    }
}

