using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _healthText;

    void Start()
    {
        Player.Current.OnDamageTaken += UpdateHealthText;
    }

    void UpdateHealthText()
    {
        _healthText.text = "Health: " + Player.Current.Health.ToString("F0") + "/" + Player.Current.MaxHealth.ToString("F0");
    }
}
