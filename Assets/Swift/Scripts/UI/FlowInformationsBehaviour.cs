using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift
{
    public class FlowInformationsBehaviour : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI ProductText;
        public TextMeshProUGUI Distance;
        public TextMeshProUGUI Volume;
        public TextMeshProUGUI AnnualDistance;

        void Start()
        {
        }

        public string ProductValue {
            get {
                return ProductText.text;
            }
            set {
                ProductText.text = value;
                button.GetComponent<Image>().color = FlowManager.Instance.productColor[value];
            }
        }

        public string DistanceValue
        {
            get
            {
                return Distance.text;
            }
            set
            {
                Distance.text = value;

            }
        }

        public string VolumeValue
        {
            get
            {
                return Volume.text;
            }
            set
            {
                Volume.text = value;
            }
        }

        public string AnnualDistanceValue
        {
            get
            {
                return AnnualDistance.text;
            }
        }

        private string UpdateAnnualDistanceValue()
        {
            return DistanceValue;
        }
    }
}
