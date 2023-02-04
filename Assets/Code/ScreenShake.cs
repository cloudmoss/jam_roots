using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    float intensity;
    float duration;
    float damp;
    Vector3 startPos;

    [SerializeField] private Camera mainCamera;

    void Start()
    {
        startPos = mainCamera.transform.localPosition;
    }

    // (0.7f, 0.5f, 2f) tested
    public void Shake(float intensity, float duration, float damp)
    {
        this.intensity = intensity;
        this.duration = duration;
        this.damp = damp;
    }

    void Update()
    {
        if (duration > 0)
        {
            mainCamera.transform.localPosition = startPos + Random.insideUnitSphere * intensity;
            duration -= Time.deltaTime * damp;
        } else if (mainCamera.transform.localPosition != startPos)
        {
            mainCamera.transform.localPosition = startPos;
        }
    }
}
