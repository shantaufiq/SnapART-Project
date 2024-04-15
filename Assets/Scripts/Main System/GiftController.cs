using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GiftController : MonoBehaviour
{
    public List<Image> displayImage;
    public float duration = 1.0f;

    [SerializeField] private List<Sprite> images;
    private Coroutine imageCycleCoroutine;

    void Start()
    {
        // Mulai coroutine untuk mengganti gambar
        // imageCycleCoroutine = StartCoroutine(CycleImages());
    }

    public void SetupGift(List<Sprite> imgs)
    {
        foreach (var item in imgs)
        {
            images.Add(item);
        }

        StartCoroutine(CycleImages());
    }

    IEnumerator CycleImages()
    {
        int index = 0;
        while (true)
        {
            foreach (var item in displayImage)
            {
                item.sprite = images[index];
            }

            yield return new WaitForSeconds(duration);

            index = (index + 1) % images.Count;
        }
    }

    public void StopCyclingImages()
    {
        if (imageCycleCoroutine != null)
        {
            StopCoroutine(imageCycleCoroutine);
        }
    }
}
