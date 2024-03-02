using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainCaptureSystem : MonoBehaviour
{
    public static MainCaptureSystem Instance;

    [Header("Profile Data")]
    public string FileName;
    public string SavePath;

    [Space]
    [Header("Frame Section")]
    public int frameIndex = 0;
    public List<Sprite> framePreviewSource;
    public List<Image> frameSelectionPreview;
    public List<Image> framePreviewTarget;

    [Space]
    [Header("AR Selection")]
    public int videoIndex = 0;
    public VideoPlayer videoPlayer;

    [Space]
    [Header("Main Components")]
    public GameObject UICaptureComponents;
    public GameObject panelSelectFrame;
    public GameObject CapturePanel;
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

    #region Frame-Selection 
    public void SetFrameIndex(int index)
    {
        frameIndex = index;

        foreach (var item in frameSelectionPreview)
        {
            item.sprite = framePreviewSource[frameIndex];
        }
    }
    #endregion

    #region Video-Selection
    public void SetVideoClip(VideoClip clip)
    {
        videoPlayer.clip = clip;
    }
    #endregion

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

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
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

            foreach (var item in framePreviewTarget)
            {
                item.sprite = framePreviewSource[frameIndex];
            }

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

            PrintHandler.Instance.SetFoto(capturedImages, framePreviewSource[frameIndex]);
        }
        else
        {
            UICaptureComponents.SetActive(true);
        }
    }
}
