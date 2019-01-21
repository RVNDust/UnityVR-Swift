using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift.Data
{
    public class JsonUtils : MonoBehaviour {

        public static JsonUtils Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                    return null;
            }
            private set
            {
                if (instance == null)
                    instance = value;
            }
        }
        private static JsonUtils instance;

        private void Awake()
        {
            Instance = this;
        }
        

        /// <summary>
        /// Generates a file name using the following convention : Swift YYYY MM DD – HH mm ss
        /// </summary>
        /// <returns>string fileName</returns>
        public string GenerateFileName(string baseName = "Swift - ", string extension = ".json")
        {
            string fileName = baseName;
            DateTime date = DateTime.Now;
            fileName += date.ToString("yyyy MM dd - HH mm ss");
            fileName += extension;
            return fileName;
        }

        /// <summary>
        /// Used to save a JSON File
        /// </summary>
        /// <param name="folderPath">Folder path of the saved file</param>
        /// <param name="fileName">Filename to be used</param>
        /// <param name="serializedObject">Serialized object to save</param>
        public void SaveToJson(string folderPath, string fileName, object serializedObject)
        {
            string jsonContent = "";
            jsonContent = JsonUtility.ToJson(serializedObject);
            File.WriteAllText(folderPath + fileName, jsonContent);
        }

        /// <summary>
        /// Used to load  a JSON File
        /// </summary>
        /// <param name="filePath">Path of the file we want to load</param>
        /// <returns>Content of the json file</returns>
        public string LoadFromJson(string filePath)
        {
            if(File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return "";
        }
    }
}
