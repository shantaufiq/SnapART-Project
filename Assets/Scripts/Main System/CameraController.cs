using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraController : MonoBehaviour
{
    int _currentCamIndex = 0;
    WebCamTexture _tex;
    public RawImage display;
    public TextMeshProUGUI camConditionText;

    private void Start()
    {
        // OnOffCamera_Clicked();
    }

    public void SwitchCamera_Clicked()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            _currentCamIndex += 1;
            _currentCamIndex %= WebCamTexture.devices.Length;
        }

        if (_tex != null)
        {
            StopWebcam();
            OnOffCamera_Clicked();
        }
    }

    public void OnOffCamera_Clicked()
    {
        if (_tex != null)
        {
            StopWebcam();

            camConditionText.text = "Start Camera";
        }
        else
        {
            WebCamDevice device = WebCamTexture.devices[_currentCamIndex];
            _tex = new WebCamTexture(device.name);
            display.texture = _tex;

            _tex.Play();

            camConditionText.text = "Stop Camera";
        }
    }

    private void StopWebcam()
    {
        display.texture = null;
        _tex.Stop();
        _tex = null;
    }
}
