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
        public Product productType;
        public TextMeshProUGUI ProductText;
        public TextMeshProUGUI Distance;
        public TextMeshProUGUI Volume;
        public TextMeshProUGUI AnnualDistance;

        void Start()
        {
        }

        public Product ProductValue {
            get {
                return productType;
            }
            set {
                productType = value;
                ProductText.text = Enum.GetName(typeof(Product), productType);
                button.GetComponent<Image>().color = FlowManager.Instance.productColor[productType];
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
