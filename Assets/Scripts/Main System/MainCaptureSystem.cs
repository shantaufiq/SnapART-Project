using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;

public class MainCaptureSystem : MonoBehaviour
{
    public static MainCaptureSystem Instance;

    [Header("Profile Data")]
    public string EventName;
    public string SavePath;
    public string customerName;

    [Space]
    [Header("Frame Section")]
    public int frameIndex = 0;
    public List<Sprite> framePreviewSource;
    public List<Image> frameSelectionPreview;
    public List<Image> framePreviewTarget;
    public UnityEvent onUserStartedPhoto;

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
    public CameraController camController;

    [Space]
    [Header("Photo Preview")]
    private List<Texture2D> capturedImages = new();
    public GameObject prevewPhotoParent;
    public List<Image> previewImg;
    public List<Button> retakeButtons;
    public List<Image> imagePlacements;
    public GiftController giftController;
    private int captureCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        EventName = DataStorageSystem.GetEventName();
        SavePath = DataStorageSystem.GetFolderLocation();
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
    public void SetVideoClip(VideoClip clip = null)
    {
        if (clip == null)
        {
            videoPlayer.gameObject.SetActive(false);
            videoPlayer.Stop();
        }
        else videoPlayer.clip = clip;
    }
    #endregion

    public void OnClickScreenShoot()
    {
        StartCoroutine(CountdownToScreenshoot(countdownTime));
    }

    IEnumerator CountdownToScreenshoot(float countdownValue)
    {
        prevewPhotoParent.SetActive(false);

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
        string fullPath = $"{tempPath}/{EventName}_{customerName}_{captureCount}_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg";
        byte[] imageBytes = screenImage.EncodeToJPG();

        File.WriteAllBytes(fullPath, imageBytes);

        yield return new WaitForSeconds(.5f);

        prevewPhotoParent.SetActive(true);

        if (captureCount < 3)
        {
            byte[] thisImageBytes = File.ReadAllBytes(fullPath);
            Texture2D currentPhoto = new Texture2D(Screen.width, Screen.height);
            currentPhoto.LoadImage(thisImageBytes);

            capturedImages.Add(currentPhoto);
            previewImg[captureCount].sprite = CreateSpriteFromTexture(capturedImages[captureCount]);

            for (int i = 0; i < retakeButtons.Count; i++)
            {
                if (i == captureCount) retakeButtons[i].gameObject.SetActive(true);
                else retakeButtons[i].gameObject.SetActive(false);
            }

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
                    imagePlacements[i].sprite = CreateSpriteFromTexture(capturedImages[i]);
                    imagePlacements[secondPlacementIndex].sprite = CreateSpriteFromTexture(capturedImages[i]);

                    List<Sprite> tempSprites = new();
                    tempSprites.Add(CreateSpriteFromTexture(capturedImages[i]));
                    giftController.SetupGift(tempSprites);
                }
            }

            PrintHandler.Instance.SetFoto(capturedImages, framePreviewSource[frameIndex]);

            camController.CameraOff();
        }
        else
        {
            UICaptureComponents.SetActive(true);
        }
    }

    // for set customer name
    public void SetCustomerName(TMP_InputField tmpInput)
    {
        if (string.IsNullOrEmpty(tmpInput.text)) return;

        onUserStartedPhoto.Invoke();
        customerName = tmpInput.text;
        Debug.Log($"Current customer name: {tmpInput.text}");
    }

    public void RetakePhoto()
    {
        // RemoveItemAtIndex(capturedImages, captureCount);

        // capturedImages.RemoveAt(captureCount);

        Debug.Log("dibawah adalah jumlahnya");
        Debug.Log(capturedImages.Count);

        for (int i = 0; i < previewImg.Count; i++)
        {
            if (previewImg[i] != null)
            {
                if (i < capturedImages.Count)
                {
                    previewImg[i].sprite = CreateSpriteFromTexture(capturedImages[i]);
                }
                else
                {
                    previewImg[i].sprite = null;
                }
            }
            else
            {
                Debug.LogWarning("Image component pada index " + i + " adalah null.");
            }
        }
    }

    public static void RemoveItemAtIndex<T>(List<T> list, int index)
    {
        // Check if the index is within the range of the list
        if (index >= 0 && index < list.Count)
        {
            list.RemoveAt(index);
        }
        else
        {
            Console.WriteLine("Index out of range. No item removed.");
        }
    }

    public static Sprite CreateSpriteFromTexture(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("Texture is null. Cannot create sprite.");
            return null;
        }

        return Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100.0f);
    }
}
