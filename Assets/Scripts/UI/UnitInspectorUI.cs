using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInspectorUI : MonoBehaviour
{
    public static UnitInspectorUI Current { get; private set; }
    public static Entity CurrentEntity { get; private set; }

    [SerializeField] private Text _title;
    [SerializeField] private Text _healthText;
    [SerializeField] private Image _healthBar;

    private bool _isOpen;

    private void Awake() {
        Current = this;
    }

    void Start()
    {
        Close();
        StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        while (true)
        {
            if (_isOpen)
            {
                _healthText.text = string.Format("Health: {0}/{1}", CurrentEntity.Health.ToString("F0"), CurrentEntity.MaxHealth.ToString("F0"));
                _healthBar.fillAmount = CurrentEntity.Health / CurrentEntity.MaxHealth;
            }

            yield return new WaitForSeconds(0.15f);
        }
    }

    public void Open(Entity entity)
    {
        CurrentEntity = entity;
        _isOpen = true;
        Populate();
    }

    public void Close()
    {
        _isOpen = false;
    }

    void Populate()
    {
        _title.text = CurrentEntity.Name;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _isOpen ? Vector3.one : Vector3.zero, Time.deltaTime * 20f);
    }
}
