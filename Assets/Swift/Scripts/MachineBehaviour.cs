using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBehaviour : MonoBehaviour {

    GameObject lightbulb;
    public Material highlight;
    public Material unhighlight;

    void Start()
    {
        lightbulb = transform.Find("Lightbulb").gameObject;
    }

	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Electricity"))
        {
            lightbulb.GetComponent<Renderer>().material = highlight;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Electricity"))
        {
            lightbulb.GetComponent<Renderer>().material = unhighlight;
        }
    }
}
