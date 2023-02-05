using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Current { get; private set; }

    [SerializeField] private Text _healthText;
    [SerializeField] private Text _waveText;
    [SerializeField] private Image _healthBar;

    private void Awake() {
        Current = this;
    }

    void Start()
    {
        Player.Current.OnDamageTaken += (float f) => UpdateHealthText();
        Player.Current.OnHeal += UpdateHealthText;
        StartCoroutine(UpdateWaveText());
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        _healthText.text = "Health: " + Player.Current.Health.ToString("F0") + "/" + Player.Current.MaxHealth.ToString("F0");
        _healthBar.fillAmount = Player.Current.Health / Player.Current.MaxHealth;
    }

    IEnumerator UpdateWaveText()
    {
        while(true)
        {
            System.TimeSpan ts = System.TimeSpan.FromSeconds(EnemyController.Current.WaveTimer);
            var timer = ts.ToString("mm\\:ss");
            _waveText.text = string.Format("Wave: {0}\n<size=20>Next wave in {1}</size>", EnemyController.Current.Wave, timer);

            yield return new WaitForSeconds(0.15f);
        }
    }
}
