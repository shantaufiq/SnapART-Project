using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// for android
public class CaptureHandler : MonoBehaviour
{
    public string FileName;
    public RenderTexture RT;
    public GameObject RenderCamera;
    public RawImage RI;


    void getImage()
    {
        Texture2D texture2D = new Texture2D(RT.width, RT.height);
        RenderTexture.active = RT;
        texture2D.ReadPixels(new Rect(0, 0, RT.width, RT.height), 0, 0);
        texture2D.Apply();

        string Path = Application.persistentDataPath + "/" + FileName + ".png";
        byte[] bytes = texture2D.EncodeToPNG();

        File.WriteAllBytes(Path, bytes);


    }

    void setImage()
    {
        Texture2D texture2D = new Texture2D(RT.width, RT.height);
        string path = Application.persistentDataPath + "/" + FileName + ".png";
        byte[] bytes = File.ReadAllBytes(path);

        texture2D.LoadImage(bytes);
        texture2D.Apply();
        RI.texture = texture2D;
    }
    IEnumerator RenderProcess()
    {
        RenderCamera.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        getImage();
        yield return new WaitForSeconds(0.1f);
        RI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        setImage();
        yield return new WaitForSeconds(0.3f);
        RenderCamera.SetActive(false);
    }
    public void GetSetImage_btn()
    {
        StartCoroutine(RenderProcess());
    }
}
