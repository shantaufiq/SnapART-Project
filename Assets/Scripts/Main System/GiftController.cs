using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GiftController : MonoBehaviour
{
    public Image displayImage;
    public float duration = 1.0f;

    [SerializeField] private Sprite[] images;
    private Coroutine imageCycleCoroutine;

    void Start()
    {
        // Mulai coroutine untuk mengganti gambar
        imageCycleCoroutine = StartCoroutine(CycleImages());
    }

    IEnumerator CycleImages()
    {
        int index = 0;
        while (true)
        {
            displayImage.sprite = images[index];

            yield return new WaitForSeconds(duration);

            index = (index + 1) % images.Length;
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
