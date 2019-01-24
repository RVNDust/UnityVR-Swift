using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift.UI
{
    public class WindowsManager : MonoBehaviour
    {
        Transform cameraPos;
        Dictionary<GameObject,GameObject> windowsList = new Dictionary<GameObject, GameObject>();

        // Use this for initialization
        void Start()
        {
            cameraPos = transform.Find("Camera");
        }

        public GameObject CreateWindow(GameObject goRef, GameObject container = null, bool placement = true)
        {
            if(container == null)
            {
                container = gameObject;
            }
            if (!windowsList.ContainsKey(goRef))
            {
                GameObject window = Instantiate(goRef, container.transform);

                if (window.GetComponent<LookAt>() == null)
                    window.AddComponent<LookAt>().target = cameraPos;
                if(placement)
                    PlaceWindow(window);
                
                windowsList.Add(goRef, window);
            }
            else
            {
                ToggleWindowState(goRef, true);
                if(placement)
                    PlaceWindow(windowsList[goRef]);
            }

            return windowsList[goRef];
        }

        private void PlaceWindow(GameObject window)
        {
            Vector3 basePos = Vector3.zero;
            basePos.x = cameraPos.localPosition.x;
            basePos.z = cameraPos.localPosition.z;

            basePos += cameraPos.forward;
            basePos.y = 1;

            window.transform.localPosition = basePos;
        }

        public void ToggleWindowState(GameObject goRef, bool state)
        {
            if (windowsList.ContainsKey(goRef))
            {
                windowsList[goRef].SetActive(state);
            }
        }

        public void DestroyWindow(GameObject goRef)
        {
            if(windowsList.ContainsKey(goRef))
            {
                GameObject window = windowsList[goRef];
                windowsList.Remove(goRef);
                Destroy(window);
            }
        }
    }
}