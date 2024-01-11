using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenCapture : MonoBehaviour
{
    public string FileName;
    public GameObject CapturePanel;

    public void OnClickScreenshot()
    {

        Invoke("Screenshot", 0.25f);
    }

    void Screenshot()
    {
        Debug.Log("click Screenshot");
        StartCoroutine(TakeScreenshot());
    }

    IEnumerator TakeScreenshot()
    {
        Debug.Log("you has been captured");

        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        string Path = Application.persistentDataPath + "/" + FileName + ".png";
        Debug.Log(Path);
        byte[] imageBytes = screenImage.EncodeToPNG();

        File.WriteAllBytes(Path, imageBytes);

        yield return new WaitForSeconds(0.5f);

        // CapturePanel.SetActive(true);
    }
}
