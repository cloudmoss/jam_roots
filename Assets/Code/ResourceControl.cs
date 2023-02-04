using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class ResourceClass
{
    public string Name { get { return _name; } }
    public int Amount { get { return _amount; } set { _amount = value; } }
    public Sprite Sprite { get { return _sprite; } }

    [SerializeField] private string _name;
    [SerializeField] private int _amount;
    [SerializeField] private Sprite _sprite;

    public ResourceClass(string name, int amount)
    {
        _name = name;
        _amount = amount;
    }
}

public class ResourceControl : MonoBehaviour
{
    public static ResourceControl Current { get; private set; }

    [SerializeField] private ResourceClass[] _resources;
    [SerializeField] private GameObject _resourcePrefab;

    private Dictionary<ResourceClass, Text> _resourceTexts = new Dictionary<ResourceClass, Text>();
    private Dictionary<string, Coroutine> _bounceCoros = new Dictionary<string, Coroutine>();
    private Color _resourceColor;

    private void Awake() 
    {
        Current = this;
        _resourceColor = _resourcePrefab.GetComponentInChildren<Text>().color;
    }

    void Start()
    {
        Populate();
        _resourcePrefab.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddResources("Biomass", 25);
        }
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
            _bounceCoros.Add(resource.Name, null);
        }
    }

    public bool TrySpend(ResourceClass[] costs)
    {
        if (CanAfford(costs))
        {
            foreach (var cost in costs)
            {
                UseResources(cost.Name, cost.Amount);
            }

            return true;
        }
        else
        {
            FlashResourceText(costs);
            return false;
        }
    }

    void UpdateResourceText(string resource)
    {
        var rclass = _resources.FirstOrDefault(r => r.Name == resource);
        _resourceTexts[rclass].text = string.Format("{0}: {1}", resource, rclass.Amount);
    }

    public void AddResources(string resource, int amount)
    {
        _resources.FirstOrDefault(r => r.Name == resource).Amount += amount;
        UpdateResourceText(resource);

        if (_bounceCoros[resource] == null)
            _bounceCoros[resource] = StartCoroutine(BounceResourceTextCoroutine(resource));
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

    public bool CanAfford(ResourceClass[] resources)
    {
        foreach (var resource in resources)
        {
            if (!CanAfford(resource.Name, resource.Amount))
                return false;
        }

        return true;
    }

    public void FlashResourceText(ResourceClass[] resources)
    {
        foreach (var resource in resources)
        {
            StartCoroutine(FlashResourceTextCoroutine(resource.Name));
        }
    }

    public void FlashResourceText(string resource)
    {
        StartCoroutine(FlashResourceTextCoroutine(resource));
    }

    IEnumerator FlashResourceTextCoroutine(string resource)
    {
        var rclass = _resources.FirstOrDefault(r => r.Name == resource);
        _resourceTexts[rclass].transform.localScale = Vector3.one * 1.2f;

        for (int i = 0; i < 3; i++)
        {
            _resourceTexts[rclass].color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _resourceTexts[rclass].color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        _resourceTexts[rclass].transform.localScale = Vector3.one * 1f;
        _resourceTexts[rclass].color = _resourceColor;
    }

    IEnumerator BounceResourceTextCoroutine(string resource)
    {
        var rclass = _resources.FirstOrDefault(r => r.Name == resource);
        
        _resourceTexts[rclass].transform.localScale = Vector3.one * 1.2f;

        var t = 0f;
        while (t < 1f)
        {
            var e = t < 0.5f ? EaseOutElastic(t * 2f, 0, 1, 1) : EaseOutElastic(1 - ((t - 0.5f) * 2f), 0, 1, 1);
            _resourceTexts[rclass].transform.localScale = Vector3.one + (Vector3.one * 0.5f * e);
            t += Time.deltaTime * 2f;
            yield return null;
        }

        _resourceTexts[rclass].transform.localScale = Vector3.one;

        _bounceCoros[resource] = null;
    }

    // Elastic ease out
    // t = current time
    // b = start value
    // c = change in value
    // d = duration
    float EaseOutElastic(float t, float b, float c, float d)
    {
        float s = 1.70158f;
        float p = 0;
        float a = c;

        if (t == 0) return b;
        if ((t /= d) == 1) return b + c;
        if (p == 0) p = d * 0.3f;
        if (a < Mathf.Abs(c))
        {
            a = c;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
        }

        return a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b;
    }
}
