using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    public static ScreenshotHandler Instance;

    public Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    public List<Image> targetImageComponents;

    private void Awake()
    {
        Instance = this;
        // myCamera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToJPG();
            string path = $"{ScreenCapture.Instance.SavePath}/Customer_{ScreenCapture.Instance.FileName}_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg";
            File.WriteAllBytes(path, byteArray);
            Debug.Log(path);

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height);
        takeScreenshotOnNextFrame = true;
    }

    public void TakeScreenshot_Function()
    {
        TakeScreenshot(1800, 1205);
    }

    public void SetFoto(List<Texture2D> imgResult)
    {
        for (int i = 0; i < targetImageComponents.Count; i++)
        {
            if (i < imgResult.Count && targetImageComponents[i] != null)
            {
                Texture2D texture = imgResult[i];
                Sprite newSprite = Sprite.Create(
                    texture,
                    new Rect(0.0f, 0.0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    100.0f);

                targetImageComponents[i].sprite = newSprite;
            }
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     TakeScreenshot(1800, 1205);
        // }
    }
}
