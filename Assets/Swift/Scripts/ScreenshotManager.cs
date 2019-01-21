using Swift.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Swift 
{
    public class ScreenshotManager : MonoBehaviour
    {
        Camera currentView;
        bool IsTakingScreenshot = false;
        string screenFolderPath;
        int captureWidth = 400, captureHeight = 400;

        void Start()
        {
            ConfigData.SavingPaths savingPaths = ConfigData.Instance.LoadConfigData(ConfigElement.Paths) as ConfigData.SavingPaths;
            screenFolderPath = Application.streamingAssetsPath + savingPaths.Screenshots;

            if (!Directory.Exists(screenFolderPath))
            {
                Directory.CreateDirectory(screenFolderPath);
            }
        }
        

        void OnPostRender()
        {
            if (IsTakingScreenshot)
            {
                IsTakingScreenshot = false;
                RenderTexture renderTexture = currentView.targetTexture;

                Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
                Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
                renderResult.ReadPixels(rect, 0, 0);

                string fileName = JsonUtils.Instance.GenerateFileName("Swift ", ".png");

                byte[] byteArray = renderResult.EncodeToPNG();
                File.WriteAllBytes(screenFolderPath + fileName, byteArray);

                RenderTexture.ReleaseTemporary(renderTexture);
                currentView.targetTexture = null;
                //currentView = null;
            }

        }

        public void TakeScreenshot(Camera view)
        {
            view.targetTexture = RenderTexture.GetTemporary(captureWidth, captureHeight, 16);
            currentView = view;
            IsTakingScreenshot = true;
        }
    }
}

