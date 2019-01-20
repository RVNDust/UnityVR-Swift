using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Swift 
{
    public class ScreenshotManager : MonoBehaviour
    {

        public static ScreenshotManager Instance { get; private set; }

        Camera currentView;
        bool IsTakingScreenshot = false;

        void Awake()
        {
            Instance = this;
        }

        void OnPostRenderer()
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
                File.WriteAllBytes(Application.dataPath + "/Screenshots/" + fileName, byteArray);

                RenderTexture.ReleaseTemporary(renderTexture);
                currentView.targetTexture = null;
                currentView = null;
            }

        }

        public void TakeScreenshot(Camera view, int width, int height)
        {
            view.targetTexture = RenderTexture.GetTemporary(width, height, 16);
            currentView = view;
            IsTakingScreenshot = true;
        }
    }
}

