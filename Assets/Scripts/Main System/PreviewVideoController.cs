using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreviewVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    private int videoIndex = -1;
    private GameObject ARSelectionPanel;

    // [Header("Capture Section")]
    // public GameObject CapturePanel;
    // public MainCaptureSystem mainCaptureSystem;

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


        videoPlayer.clip = videoClips[clipIndex];
        videoPlayer.Play();

        videoPlayer.gameObject.SetActive(true);
        videoIndex = clipIndex;
    }

    public void CloseVideoPlayer()
    {
        videoPlayer.gameObject.SetActive(false);
        videoPlayer.Stop();
    }

    // for choosing AR from another panel
    public void OnClickVideoReady()
    {
        // CapturePanel.SetActive(true);

        // if (videoIndex > -1)
        //     mainCaptureSystem.SetVideoClip(videoClips[videoIndex]);
        // else
        //     mainCaptureSystem.SetVideoClip();

        ARSelectionPanel.SetActive(false);
        videoPlayer.clip = null;
        videoIndex = -1;
    }
}
