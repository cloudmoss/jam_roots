using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class ResourceClass
{
    public string Name { get { return _name; } }
    public int Amount { get; set; }
    public Sprite Sprite { get { return _sprite; } }

    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
}

public class ResourceControl : MonoBehaviour
{
    public static ResourceControl Current { get; private set; }

    [SerializeField] private ResourceClass[] _resources;
    [SerializeField] private GameObject _resourcePrefab;

    private Dictionary<ResourceClass, Text> _resourceTexts = new Dictionary<ResourceClass, Text>();

    private void Awake() 
    {
        Current = this;
    }

    void Start()
    {
        Populate();
        _resourcePrefab.SetActive(false);
    }

    void Populate()
    {
        foreach (var resource in _resources)
        {
            var resourceObject = Instantiate(_resourcePrefab, _resourcePrefab.transform.parent);
            resourceObject.GetComponentInChildren<Image>().sprite = resource.Sprite;
            var txt = resourceObject.GetComponentInChildren<Text>();
            txt.text = string.Format("{0}: {1}", resource.Name, resource.Amount);

            _resourceTexts.Add(resource, txt);
        }
    }

    void UpdateResourceText(string resource)
    {
        // Ugly af
        var rclass = _resources.FirstOrDefault(r => r.Name == resource);
        _resourceTexts[rclass].text = string.Format("{0}: {1}", resource, rclass.Amount);
    }

    public void AddResources(string resource, int amount)
    {
        _resources.FirstOrDefault(r => r.Name == resource).Amount += amount;
        UpdateResourceText(resource);
    }

    public void UseResources(string resource, int amount)
    {
        _resources.FirstOrDefault(r => r.Name == resource).Amount -= amount;
        UpdateResourceText(resource);
    }

    public bool CanAfford(string resource, int price)
    { 
        return _resources.FirstOrDefault(r => r.Name == resource).Amount >= price;
    }

    public void FlashResourceText(string resource)
    {
        StartCoroutine(FlashResourceTextCoroutine(resource));
    }

    IEnumerator FlashResourceTextCoroutine(string resource)
    {
        var rclass = _resources.FirstOrDefault(r => r.Name == resource);
        var originalColor = _resourceTexts[rclass].color;
        _resourceTexts[rclass].transform.localScale = Vector3.one * 1.2f;

        for (int i = 0; i < 3; i++)
        {
            _resourceTexts[rclass].color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _resourceTexts[rclass].color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        _resourceTexts[rclass].transform.localScale = Vector3.one * 1f;
        _resourceTexts[rclass].color = originalColor;
    }
}
