using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private int maxBloodSplatters = 10;
    private float spreadDistance = 0.1f;
    private float minScale = 0.01f;
    private float maxScale = 0.1f;

    private void Update()
    {
        // FOR TESTING ONLY 
        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            BloodSplat(mousePos);
        }
    }

    void BloodSplat(Vector3 bloodPos)
    {
        // Randomly generate the blood splatters
        for (int i = 0; i < maxBloodSplatters; i++)
        {
            float x = Random.Range(-spreadDistance, spreadDistance);
            float y = Random.Range(-spreadDistance, spreadDistance);
            GameObject blood = Instantiate(bloodPrefab, bloodPos + new Vector3(x, y, 0), Quaternion.identity);
            float scale = Random.Range(minScale, maxScale);
            blood.transform.localScale = new Vector3(scale, scale, 1);
            blood.AddComponent<Spread>();

            // Randomly rotate the blood splatter
            float rotation = Random.Range(0, 360);
            blood.transform.Rotate(0, 0, rotation);
        }
    }
}