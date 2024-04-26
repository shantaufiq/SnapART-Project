using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPreview : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private GameObject imgNumber;
    public Sprite img { get => image.sprite; }
    public bool isNullImage;

    public void SetPhotoPreview(Sprite img)
    {
        image.sprite = img;
        isNullImage = true;

        imgNumber.SetActive(false);
    }

    public void RetakePhoto()
    {
        image.sprite = null;
        isNullImage = false;

        imgNumber.SetActive(true);
    }
}
