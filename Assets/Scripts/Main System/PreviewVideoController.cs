using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreviewVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;

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
    }

    public void OnClickVideoReady(int index)
    {

    }
}
