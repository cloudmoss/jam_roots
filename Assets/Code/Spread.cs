using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : MonoBehaviour
{
    private float growthSpeed = 10f;
    private float maxSize = 1.6f;

    private Vector3 targetScale;
    private Vector3 startScale;

    Texture2D[] bloodTextures;

    private void Start()
    {
        startScale = transform.localScale;
        bloodTextures = System.Array.ConvertAll(Resources.LoadAll("Blood Textures", typeof(Texture2D)), o => (Texture2D)o);
        int randomTexture = Random.Range(0, bloodTextures.Length);
        GetComponent<MeshRenderer>().material.mainTexture = bloodTextures[randomTexture];
        float targetX = Random.Range(startScale.x, maxSize);
        float targetY = Random.Range(startScale.y, maxSize);
        float targetZ = startScale.z;
        targetScale = new Vector3(targetX, targetY, targetZ);
    }

    private void Update()
    {
        float sizeDifference = Vector3.Distance(transform.localScale, targetScale);
        float ease = Mathf.SmoothStep(0.0f, 1.0f, sizeDifference / maxSize);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growthSpeed * ease * Time.deltaTime);

        if (transform.localScale == targetScale)
        {
            enabled = false;
        }
    }
}