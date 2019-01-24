using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swift.UI
{
    public class TargetStateCanvasBehaviour : CanvasBehaviour
    {
        string folderPath;
        string fileName;

        public Image img;

        // Use this for initialization
        void Start()
        {
            StartBehaviour();
        }

        // Update is called once per frame
        void Awake()
        {
            AwakeBehaviour();
            ConfigData.SavingPaths paths = ConfigData.Instance.LoadConfigData(ConfigElement.Paths) as ConfigData.SavingPaths;
            fileName = Application.streamingAssetsPath + paths.TargetStateFile;
            WWW www = new WWW(fileName);
            while(!www.isDone)
            {
            }

            img.overrideSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f,0.5f));
        }

        void Update()
        {

        }
    }
}
