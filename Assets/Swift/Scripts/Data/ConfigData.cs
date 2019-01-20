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
        public class SavingPathes
        {
            public string TargetStateFile;
            public string Layouts;
            public string Screenshots;
        }

        [Serializable]
        public class RootObject
        {
            public Flows Flows;
            public SavingPathes SavingPathes;
        }

        public static ConfigData Instance { get; private set; }
        string configFilePath = Application.dataPath + "/ConfigFile.json";

        void Awake()
        {
            Instance = this;
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
                    case ConfigElement.Pathes:
                        return config.SavingPathes;
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
        Pathes
    }
}
