using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolInfoBehaviour : MonoBehaviour {

    public TextMeshProUGUI text;
    public Image icon;

    public void UpdateInfo(string name, Sprite sprite)
    {
        text.text = "";
        icon.sprite = sprite;
    }

    public void UpdateRotation(float angle)
    {
        gameObject.transform.Rotate(new Vector3(0, 0, 1), angle, Space.Self);
    }
}
