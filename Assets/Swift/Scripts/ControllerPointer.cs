using UnityEngine;

namespace Swift
{
    public class ControllerPointer : MonoBehaviour
    {
        public Vector3 TargetPosition;
        public bool CanTeleport;
        public Color Color;
        public float Thickness = 0.002f;
        public float Length = 100f;
        public int TeleportMask;

        GameObject holder;
        GameObject pointer;
        GameObject cursor;

        Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);
        float contactDistance = 0f;
        Transform contactTarget = null;

        void SetPointerTransform(float setLength, float setThicknes)
        {
            float beamPosition = setLength / (2 + 0.00001f);

            pointer.transform.localScale = new Vector3(setThicknes, setThicknes, setLength);
            pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);
            cursor.transform.localPosition = new Vector3(0f, 0f, setLength);
        }

        // Use this for initialization
        void Start()
        {
            TeleportMask = LayerMask.GetMask("CanTeleport");
            ActivatePointer();
        }

        float GetBeamLength(bool bHit, RaycastHit hit)
        {
            float actualLength = Length;

            if (!bHit || (contactTarget && contactTarget != hit.transform))
            {
                contactDistance = 0f;
                contactTarget = null;
            }
            if (bHit)
            {
                if (hit.distance <= 0)
                {

                }
                contactDistance = hit.distance;
                contactTarget = hit.transform;
            }

            if (bHit && contactDistance < Length)
            {
                actualLength = contactDistance;
            }

            if (actualLength <= 0)
            {
                actualLength = Length;
            }

            return actualLength; ;
        }

        void Update()
        {
            Ray raycast = new Ray(transform.position, transform.forward);

            RaycastHit hitObject;
            bool rayHit = Physics.Raycast(raycast, out hitObject, Mathf.Infinity, TeleportMask);
            if (rayHit)
            {
                CanTeleport = true;
                TargetPosition = hitObject.point;
                UpdateColor(Color.green);
            }
            else
            {
                CanTeleport = false;
                UpdateColor(Color.red);
            }

            float beamLength = GetBeamLength(rayHit, hitObject);
            SetPointerTransform(beamLength, Thickness);
        }

        public void UpdateColor(Color color)
        {
            if(pointer != null)
                pointer.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
            if(cursor != null)
                cursor.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }

        public void ActivatePointer()
        {
            Material newMaterial = new Material(Shader.Find("Unlit/Color"));
            newMaterial.SetColor("_Color", Color);

            holder = new GameObject();
            holder.name = "Pointer";
            holder.transform.parent = this.transform;
            holder.transform.localPosition = Vector3.zero;


            pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointer.name = "Laser";
            pointer.transform.parent = holder.transform;
            pointer.GetComponent<MeshRenderer>().material = newMaterial;

            pointer.GetComponent<BoxCollider>().isTrigger = true;
            pointer.AddComponent<Rigidbody>().isKinematic = true;
            pointer.layer = 2;

            cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cursor.name = "Cursor";
            cursor.transform.parent = holder.transform;
            cursor.GetComponent<MeshRenderer>().material = newMaterial;
            cursor.transform.localScale = cursorScale;

            cursor.GetComponent<SphereCollider>().isTrigger = true;
            cursor.AddComponent<Rigidbody>().isKinematic = true;
            cursor.layer = 2;
            holder.transform.localRotation = new Quaternion(0, 0, 0, 0);
            SetPointerTransform(Length, Thickness);
        }

        public void DesactivatePointer()
        {
            Destroy(holder);
            Destroy(pointer);
            Destroy(cursor);
        }
    }
}