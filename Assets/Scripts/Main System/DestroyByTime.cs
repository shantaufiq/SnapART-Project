using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float destroyTime;

    private void OnEnable()
    {
        Invoke(nameof(DestroyGameObject), destroyTime);
    }

    private void DestroyGameObject() => Destroy(gameObject);
}
