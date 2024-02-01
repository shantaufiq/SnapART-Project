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

        RectTransform rt = CapturePanel.GetComponent<RectTransform>();

        float width = rt.rect.width;
        float height = rt.rect.height;

        float x = (Screen.width / 2f) - (width / 2f);
        float y = (Screen.height / 2f) - (height / 2f);

        Texture2D screenImage = new Texture2D((int)width, (int)height);

        screenImage.ReadPixels(new Rect(x, y, width, height), 0, 0);
        screenImage.Apply();

        Debug.Log("Width: " + width + ", Height: " + height + ", X: " + x + ", Y: " + y);

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

            for (int i = 0; i < capturedImages.Count; i++)
            {
                int secondPlacementIndex = i + 3;

                if (imagePlacements[i] != null && imagePlacements.Count > secondPlacementIndex && imagePlacements[secondPlacementIndex] != null)
                {
                    Sprite newSprite = Sprite.Create(
                        capturedImages[i],
                        new Rect(0.0f, 0.0f, capturedImages[i].width, capturedImages[i].height),
                        new Vector2(0.5f, 0.5f),
                        100.0f);

                    imagePlacements[i].sprite = newSprite;
                    imagePlacements[secondPlacementIndex].sprite = newSprite;
                }
            }

            ScreenshotHandler.Instance.SetFoto(capturedImages);
        }
        else
        {
            UICaptureComponents.SetActive(true);
        }
    }
}
