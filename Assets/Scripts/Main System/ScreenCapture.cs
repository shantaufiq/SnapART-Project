using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class ScreenCapture : MonoBehaviour
{
    public static ScreenCapture Instance;

    [Header("Profile Data")]
    public string FileName;
    public string SavePath;

    [Header("Main Components")]
    public GameObject UICaptureComponents;
    public GameObject panelSelectFrame;
    public GameObject CapturePanel;
    public Image finalImage;
    public float countdownTime = 5f;
    public TextMeshProUGUI countdownText;

    [SerializeField] private List<Texture2D> capturedImages = new List<Texture2D>();
    public List<Image> imagePlacements;
    private int captureCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    public void OnClickScreenShoot()
    {
        StartCoroutine(CountdownToScreenshoot(countdownTime));
    }

    IEnumerator CountdownToScreenshoot(float countdownValue)
    {
        if (countdownText != null)
        {
            UICaptureComponents.SetActive(false);
            countdownText.gameObject.SetActive(true);
        }

        while (countdownValue > 0)
        {
            if (countdownText != null)
                countdownText.text = countdownValue.ToString();

            yield return new WaitForSeconds(1f);
            countdownValue--;
        }

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        StartCoroutine(StartCapturedImages());
    }

    IEnumerator StartCapturedImages()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        string tempPath = Path.GetTempPath();
        string fullPath = $"{tempPath}/Customer_{FileName}_{captureCount}_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg";
        byte[] imageBytes = screenImage.EncodeToJPG();

        File.WriteAllBytes(fullPath, imageBytes);

        yield return new WaitForSeconds(.5f);

        if (captureCount < 3)
        {
            byte[] thisImageBytes = File.ReadAllBytes(fullPath);
            Texture2D currentPhoto = new Texture2D(Screen.width, Screen.height);
            currentPhoto.LoadImage(thisImageBytes);

            capturedImages.Add(currentPhoto);
            captureCount++;
        }

        if (captureCount >= 3)
        {
            panelSelectFrame.SetActive(true);

            for (int i = 0; i < imagePlacements.Count; i++)
            {
                if (i < capturedImages.Count && imagePlacements[i] != null)
                {
                    Texture2D texture = capturedImages[i];
                    Sprite newSprite = Sprite.Create(
                        texture,
                        new Rect(0.0f, 0.0f, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100.0f);

                    imagePlacements[i].sprite = newSprite;
                }
            }

            ScreenshotHandler.Instance.SetFoto(capturedImages);
        }
        else
        {
            UICaptureComponents.SetActive(true);
        }

        // Debug.Log("All images saved to temporary folder: " + fullPath);
    }

    public void SaveImage()
    {
        Debug.Log("click Screenshot");
        StartCoroutine(SaveFinalImage());
    }

    IEnumerator SaveFinalImage()
    {
        Debug.Log("you has been captured");

        yield return new WaitForEndOfFrame();

        Sprite sprite = finalImage.sprite;
        Texture2D texture = SpriteToTexture2D(sprite);

        // Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        // screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // screenImage.Apply();

        // Pastikan SavePath diakhiri dengan "\"
        if (!SavePath.EndsWith(@"\")) SavePath += @"\";

        // Periksa apakah directory tersebut ada, jika tidak, buat
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        // string Path = $"{Application.persistentDataPath}/Customer_{FileName}_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg";
        string Path = $"{SavePath}/Customer_{FileName}_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg";
        Debug.Log(Path);
        byte[] imageBytes = texture.EncodeToJPG();

        File.WriteAllBytes(Path, imageBytes);

        Destroy(texture);

        yield return new WaitForSeconds(0.5f);

        CapturePanel.SetActive(true);
    }

    Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                        (int)sprite.textureRect.y,
                                                        (int)sprite.textureRect.width,
                                                        (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
