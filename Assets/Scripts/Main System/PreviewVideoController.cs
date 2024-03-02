using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreviewVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    private int videoIndex = -1;
    public GameObject ARSelectionPanel;

    [Header("Capture Section")]
    public GameObject CapturePanel;
    public MainCaptureSystem mainCaptureSystem;

    public void ChangeVideoClip(int clipIndex)
    {
        if (videoClips == null || videoClips.Length == 0)
        {
            Debug.LogError("Video clips array is empty or null!");
            return;
        }

        if (clipIndex < 0 || clipIndex >= videoClips.Length)
        {
            Debug.LogError("Clip index is out of bounds!");
            return;
        }

        videoPlayer.gameObject.SetActive(true);

        videoPlayer.clip = videoClips[clipIndex];
        videoPlayer.Play();

        videoIndex = clipIndex;
    }

    public void OnClickVideoReady()
    {
        CapturePanel.SetActive(true);

        if (videoIndex > -1)
            mainCaptureSystem.SetVideoClip(videoClips[videoIndex]);
        else
            mainCaptureSystem.SetVideoClip();

        ARSelectionPanel.SetActive(false);
        videoPlayer.clip = null;
        videoIndex = -1;
    }
}
