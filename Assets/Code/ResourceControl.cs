using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceControl : MonoBehaviour
{
    private int resourceAmount = 0;
    [SerializeField] private Text resourceText;
    private Image resourceImage;

    // Start is called before the first frame update
    void Start()
    {
        resourceImage = resourceText.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        resourceImage.fillAmount = (resourceAmount / 100f);
        resourceText.text = resourceAmount.ToString();
    }

    public void AddResources(int amount)
    {
        resourceAmount += amount;
    }

    public void UseResources(int amount)
    {
        if (CanAfford(amount))
        {
            resourceAmount -= amount;
        } else
        {
            CantAfford();
        }
    }

    public void CantAfford()
    {
        // not enough resources
        StartCoroutine(FlashTextColor(1));
    }

    public bool CanAfford(int price)
    {
        return resourceAmount >= price;
    }

    public IEnumerator FlashTextColor(float t)
    {
        resourceText.color = Color.red;
        yield return new WaitForSeconds(t);
        resourceText.color = Color.white;
    }
}
