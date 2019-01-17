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

        public Dictionary<Product, Color> productColor = new Dictionary<Product, Color>(); //TODO Change with configurable values in JSON config file

        void Start()
        {
            productColor.Add(Product.A, Color.blue);
            productColor.Add(Product.B, Color.green);
            productColor.Add(Product.C, Color.red);
            productColor.Add(Product.D, Color.yellow);
            productColor.Add(Product.E, Color.magenta);
        }

        public Product ProductValue {
            get {
                return productType;
            }
            set {
                productType = value;
                //button.GetComponent<Image>().color = productColor[productType];
                ProductText.text = productType.ToString();
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
