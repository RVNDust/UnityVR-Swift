using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift.Data
{
    public class ConfigData : MonoBehaviour
    {
        [Serializable]
        public class Product
        {
            public string Name;
            public string Color;
            public List<string> Machines;
            public int Coef;
        }

        [Serializable]
        public class Flows
        {
            public List<Product> Products;
        }

        [Serializable]
        public class SavingPaths
        {
            public string TargetStateFile;
            public string Layouts;
            public string Screenshots;
        }

        [Serializable]
        public class RootObject
        {
            public Flows Flows;
            public SavingPaths SavingPaths;
        }

        public static ConfigData Instance { get; private set; }
        string configFilePath;

        void Awake()
        {
            Instance = this;
            configFilePath = Application.streamingAssetsPath + "/ConfigFile.json";
        }

        public object LoadConfigData(ConfigElement selectedContent)
        {
            string content = JsonUtils.Instance.LoadFromJson(configFilePath);
            if(content != "")
            {
                RootObject config = JsonUtility.FromJson<RootObject>(content);
                switch(selectedContent)
                {
                    case ConfigElement.All:
                        return config;
                    case ConfigElement.Flows:
                        return config.Flows;
                    case ConfigElement.Paths:
                        return config.SavingPaths;
                    default:
                        return null;
                }
            }
            return null;
        }
    }

    public enum ConfigElement
    {
        All,
        Flows,
        Paths
    }
}
