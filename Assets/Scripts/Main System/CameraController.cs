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
        // Initialize or configure camera settings here if needed
    }

    public void SwitchCamera_Clicked()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            _currentCamIndex += 1;
            _currentCamIndex %= WebCamTexture.devices.Length;

            PlayerPrefs.SetInt("camIndex", _currentCamIndex);

            if (_tex != null)
            {
                CameraOff();
                CameraOn();
            }
        }
    }

    public void CameraOn()
    {
        WebCamDevice device = WebCamTexture.devices[PlayerPrefs.GetInt("camIndex", _currentCamIndex)];
        _tex = new WebCamTexture(device.name);
        display.texture = _tex;
        _tex.Play();
        camConditionText.text = "Stop Camera";
    }

    public void CameraOff()
    {
        if (_tex != null)
        {
            display.texture = null;
            _tex.Stop();
            _tex = null;
            camConditionText.text = "Start Camera";
        }
    }

    // The OnOffCamera_Clicked function can be removed or repurposed for other functionality.
}
